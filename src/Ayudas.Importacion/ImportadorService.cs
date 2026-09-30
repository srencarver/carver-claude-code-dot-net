using System.Xml;
using System.Xml.Linq;
using Ayudas.Application.Abstracciones;
using Ayudas.Domain.Entidades;
using Ayudas.Importacion.Lectores;
using Ayudas.Importacion.Mapeo;
using Ayudas.Importacion.Modelos;
using Ayudas.Importacion.Normalizacion;
using Ayudas.Importacion.Validacion;

namespace Ayudas.Importacion;

/// <summary>
/// Importa un fichero de remesa: lo lee con el lector de su formato, valida cabecera y solicitudes
/// y guarda la remesa con las solicitudes aceptadas.
/// </summary>
public class ImportadorService
{
    private readonly IEnumerable<IAyudasLector> _lectores;
    private readonly IEntidadRepository _entidades;
    private readonly IConvocatoriaRepository _convocatorias;
    private readonly IRemesaRepository _remesas;
    private readonly AyudaEntradaValidator _validador;

    public ImportadorService(
        IEnumerable<IAyudasLector> lectores,
        IEntidadRepository entidades,
        IConvocatoriaRepository convocatorias,
        IRemesaRepository remesas,
        AyudaEntradaValidator validador)
    {
        _lectores = lectores;
        _entidades = entidades;
        _convocatorias = convocatorias;
        _remesas = remesas;
        _validador = validador;
    }

    public async Task<ResultadoImportacion> ImportarAsync(Stream contenido, CancellationToken ct = default)
    {
        XDocument documento;
        try
        {
            // Se carga como XML (no como texto) para respetar la codificación declarada.
            documento = XDocument.Load(contenido);
        }
        catch (XmlException ex)
        {
            return ResultadoImportacion.Rechazo($"El fichero no es un XML válido: {ex.Message}");
        }

        var lector = _lectores.FirstOrDefault(l => l.Soporta(documento));
        if (lector is null)
        {
            return ResultadoImportacion.Rechazo($"Formato de fichero no soportado (raíz {documento.Root?.Name.LocalName}).");
        }

        var lectura = lector.Leer(documento);
        var cabecera = lectura.Cabecera;
        var referencia = cabecera.Referencia?.Trim();

        ResultadoImportacion Rechazo(string motivo) => ResultadoImportacion.Rechazo(motivo, referencia, lector.Formato);

        if (string.IsNullOrEmpty(referencia))
        {
            return Rechazo("La remesa no tiene referencia.");
        }

        if (await _remesas.ExisteReferenciaAsync(referencia, ct))
        {
            return Rechazo($"La remesa {referencia} ya se importó.");
        }

        var nifEntidad = NifNormalizer.Normalizar(cabecera.NifEntidad);
        var entidad = nifEntidad.EsValido ? await _entidades.ObtenerPorNifAsync(nifEntidad.Valor!, ct) : null;
        if (entidad is null || !entidad.Activa)
        {
            return Rechazo($"La entidad {cabecera.NifEntidad} no existe o no está activa.");
        }

        var codigoConvocatoria = cabecera.CodigoConvocatoria?.Trim().ToUpperInvariant() ?? string.Empty;
        var convocatoria = await _convocatorias.ObtenerPorCodigoAsync(codigoConvocatoria, ct);
        if (convocatoria is null || !convocatoria.Abierta)
        {
            return Rechazo($"La convocatoria {cabecera.CodigoConvocatoria} no existe o está cerrada.");
        }

        var fechaEnvio = FechaNormalizer.Normalizar(cabecera.FechaEnvio);
        if (!fechaEnvio.EsValido)
        {
            return Rechazo($"La fecha de envío {cabecera.FechaEnvio} no es válida.");
        }

        var contexto = new ContextoValidacion(convocatoria.FechaInicio, convocatoria.FechaFin, fechaEnvio.Valor);
        var resultado = new ResultadoImportacion
        {
            Referencia = referencia,
            Formato = lector.Formato,
            NifEntidad = entidad.Nif,
            CodigoConvocatoria = convocatoria.Codigo,
            Resultados = lectura.Solicitudes.Select(s => _validador.Validar(s, contexto)).ToList()
        };

        AnotarAvisos(lectura, resultado);

        if (resultado.Aceptadas == 0)
        {
            resultado.MotivoRechazoRemesa = "Ninguna solicitud de la remesa es válida.";
            return resultado;
        }

        var remesa = new Remesa
        {
            Referencia = referencia,
            EntidadLocalId = entidad.Id,
            ConvocatoriaId = convocatoria.Id,
            FechaEnvio = fechaEnvio.Valor.ToDateTime(TimeOnly.MinValue),
            Estado = EstadoRemesa.Recibida,
            Observaciones = resultado.Rechazadas > 0 ? $"{resultado.Rechazadas} solicitudes rechazadas en la importación" : null,
            Ayudas = resultado.Resultados.Where(r => r.EsValida).Select(r => AyudaMapper.ANueva(r.Ayuda!)).ToList()
        };

        await _remesas.AgregarAsync(remesa, ct);
        resultado.RemesaId = remesa.Id;
        return resultado;
    }

    private static void AnotarAvisos(LecturaRemesa lectura, ResultadoImportacion resultado)
    {
        if (int.TryParse(lectura.Cabecera.NumeroSolicitudes, out var declaradas) && declaradas != lectura.Solicitudes.Count)
        {
            resultado.Avisos.Add($"La cabecera declara {declaradas} solicitudes y el fichero trae {lectura.Solicitudes.Count}.");
        }

        // Hay formatos que no traen importe total: entonces no hay nada que comparar.
        if (lectura.Cabecera.ImporteTotal is not null)
        {
            var importeDeclarado = LeerImporte(lectura.Cabecera.ImporteTotal);
            if (importeDeclarado is null)
            {
                resultado.Avisos.Add($"El importe total de la cabecera ({lectura.Cabecera.ImporteTotal}) no se puede leer.");
            }
            else if (importeDeclarado != resultado.ImporteAceptado)
            {
                resultado.Avisos.Add($"El importe total de la cabecera ({importeDeclarado:N2}) no coincide con el de las solicitudes aceptadas ({resultado.ImporteAceptado:N2}).");
            }
        }

        foreach (var solicitud in lectura.Solicitudes.Where(s => s.ElementosDesconocidos.Count > 0))
        {
            resultado.Avisos.Add($"Línea {solicitud.Fila}: elementos fuera del contrato ignorados ({string.Join(", ", solicitud.ElementosDesconocidos)}).");
        }
    }

    private static decimal? LeerImporte(string? texto)
    {
        // Las entidades no siempre usan el punto decimal del contrato: mismo criterio que en las solicitudes.
        var importe = ImporteNormalizer.Normalizar(texto);
        return importe.EsValido ? importe.Valor : null;
    }
}

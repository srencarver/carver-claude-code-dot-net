using Ayudas.Importacion.Modelos;
using Ayudas.Importacion.Normalizacion;

namespace Ayudas.Importacion.Validacion;

/// <summary>
/// Normaliza y valida una solicitud según las reglas R1 a R5 de la especificación.
/// Recoge todos los rechazos de la solicitud, no solo el primero.
/// </summary>
public class AyudaEntradaValidator
{
    public const decimal ImporteMaximo = 60000.00m;

    public ResultadoValidacion Validar(AyudaEntrada entrada, ContextoValidacion contexto)
    {
        var rechazos = new List<Rechazo>();
        var correcciones = new List<string>();

        void Rechazar(string campo, string? valor, string motivo) => rechazos.Add(new Rechazo(entrada.Fila, campo, valor, motivo));

        // R1: NIF válido.
        var nif = NifNormalizer.Normalizar(entrada.NifBeneficiario);
        if (!nif.EsValido)
        {
            Rechazar(nameof(entrada.NifBeneficiario), entrada.NifBeneficiario, nif.Motivo!);
        }
        else if (nif.Corregido)
        {
            correcciones.Add(nameof(entrada.NifBeneficiario));
        }

        // R2: nombre y concepto obligatorios.
        var nombre = entrada.NombreBeneficiario?.Trim();
        if (string.IsNullOrEmpty(nombre))
        {
            Rechazar(nameof(entrada.NombreBeneficiario), entrada.NombreBeneficiario, "Campo obligatorio vacío");
        }
        else if (nombre.Length > 200)
        {
            Rechazar(nameof(entrada.NombreBeneficiario), entrada.NombreBeneficiario, "Supera los 200 caracteres");
        }

        var concepto = entrada.Concepto?.Trim();
        if (string.IsNullOrEmpty(concepto))
        {
            Rechazar(nameof(entrada.Concepto), entrada.Concepto, entrada.Concepto is null ? "Campo obligatorio ausente" : "Campo obligatorio vacío");
        }
        else if (concepto.Length > 300)
        {
            Rechazar(nameof(entrada.Concepto), entrada.Concepto, "Supera los 300 caracteres");
        }

        // R3: código de municipio de 5 dígitos.
        var municipio = entrada.CodigoMunicipio?.Trim();
        if (municipio is null || municipio.Length != 5 || !municipio.All(char.IsDigit))
        {
            Rechazar(nameof(entrada.CodigoMunicipio), entrada.CodigoMunicipio, "El código de municipio debe tener 5 dígitos");
        }
        else if (municipio != entrada.CodigoMunicipio)
        {
            correcciones.Add(nameof(entrada.CodigoMunicipio));
        }

        // R4: importe mayor que 0 y hasta 60.000 €, dos decimales como máximo.
        var importe = ImporteNormalizer.Normalizar(entrada.ImporteSolicitado);
        if (!importe.EsValido)
        {
            Rechazar(nameof(entrada.ImporteSolicitado), entrada.ImporteSolicitado, importe.Motivo!);
        }
        else if (importe.Valor <= 0)
        {
            Rechazar(nameof(entrada.ImporteSolicitado), entrada.ImporteSolicitado, "El importe debe ser mayor que 0");
        }
        else if (importe.Valor > ImporteMaximo)
        {
            Rechazar(nameof(entrada.ImporteSolicitado), entrada.ImporteSolicitado, "El importe supera el máximo de 60.000,00 €");
        }
        else if (importe.Corregido)
        {
            correcciones.Add(nameof(entrada.ImporteSolicitado));
        }

        // R5: fecha real, dentro del plazo y no posterior al envío.
        var fecha = FechaNormalizer.Normalizar(entrada.FechaSolicitud);
        if (!fecha.EsValido)
        {
            Rechazar(nameof(entrada.FechaSolicitud), entrada.FechaSolicitud, fecha.Motivo!);
        }
        else if (fecha.Valor < contexto.InicioPlazo || fecha.Valor > contexto.FinPlazo)
        {
            Rechazar(nameof(entrada.FechaSolicitud), entrada.FechaSolicitud, "Fuera del plazo de la convocatoria");
        }
        else if (fecha.Valor > contexto.FechaEnvio)
        {
            Rechazar(nameof(entrada.FechaSolicitud), entrada.FechaSolicitud, "Posterior a la fecha de envío de la remesa");
        }
        else if (fecha.Corregido)
        {
            correcciones.Add(nameof(entrada.FechaSolicitud));
        }

        if (rechazos.Count > 0)
        {
            return new ResultadoValidacion { Fila = entrada.Fila, Rechazos = rechazos };
        }

        return new ResultadoValidacion
        {
            Fila = entrada.Fila,
            Correcciones = correcciones,
            Ayuda = new AyudaNormalizada
            {
                Fila = entrada.Fila,
                NifBeneficiario = nif.Valor!,
                NombreBeneficiario = nombre!,
                CodigoMunicipio = municipio!,
                Concepto = concepto!,
                ImporteSolicitado = importe.Valor,
                FechaSolicitud = fecha.Valor
            }
        };
    }
}

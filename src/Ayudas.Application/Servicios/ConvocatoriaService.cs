using Ayudas.Application.Abstracciones;
using Ayudas.Application.Dtos;
using Ayudas.Application.Excepciones;
using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Servicios;

public class ConvocatoriaService : IConvocatoriaService
{
    private readonly IConvocatoriaRepository _convocatorias;

    public ConvocatoriaService(IConvocatoriaRepository convocatorias)
    {
        _convocatorias = convocatorias;
    }

    public async Task<List<ConvocatoriaDto>> ListarAsync(CancellationToken ct = default)
    {
        var convocatorias = await _convocatorias.ListarAsync(ct);
        return convocatorias.Select(ADto).ToList();
    }

    public async Task<ConvocatoriaDto?> ObtenerAsync(int id, CancellationToken ct = default)
    {
        var convocatoria = await _convocatorias.ObtenerAsync(id, ct);
        return convocatoria is null ? null : ADto(convocatoria);
    }

    public async Task<int> CrearAsync(ConvocatoriaDto datos, CancellationToken ct = default)
    {
        var codigo = datos.Codigo.Trim().ToUpperInvariant();
        await ValidarAsync(datos, codigo, idActual: null, ct);

        var convocatoria = new Convocatoria();
        Copiar(datos, codigo, convocatoria);
        await _convocatorias.AgregarAsync(convocatoria, ct);
        return convocatoria.Id;
    }

    public async Task ActualizarAsync(ConvocatoriaDto datos, CancellationToken ct = default)
    {
        var convocatoria = await _convocatorias.ObtenerAsync(datos.Id, ct)
            ?? throw new ValidacionException(nameof(datos.Id), "La convocatoria no existe.");

        var codigo = datos.Codigo.Trim().ToUpperInvariant();
        await ValidarAsync(datos, codigo, datos.Id, ct);

        Copiar(datos, codigo, convocatoria);
        await _convocatorias.ActualizarAsync(convocatoria, ct);
    }

    private async Task ValidarAsync(ConvocatoriaDto datos, string codigo, int? idActual, CancellationToken ct)
    {
        var existente = await _convocatorias.ObtenerPorCodigoAsync(codigo, ct);
        if (existente is not null && existente.Id != idActual)
        {
            throw new ValidacionException(nameof(datos.Codigo), "Ya existe una convocatoria con ese código.");
        }

        if (datos.PorcentajeCofinanciacion is <= 0m or > 1m)
        {
            throw new ValidacionException(nameof(datos.PorcentajeCofinanciacion), "El porcentaje de cofinanciación debe estar entre 1 y 100.");
        }

        if (datos.FechaFin < datos.FechaInicio)
        {
            throw new ValidacionException(nameof(datos.FechaFin), "La fecha de fin no puede ser anterior a la de inicio.");
        }
    }

    private static void Copiar(ConvocatoriaDto datos, string codigo, Convocatoria destino)
    {
        destino.Codigo = codigo;
        destino.Titulo = datos.Titulo.Trim();
        destino.Ejercicio = datos.Ejercicio;
        destino.PorcentajeCofinanciacion = datos.PorcentajeCofinanciacion;
        destino.FechaInicio = datos.FechaInicio;
        destino.FechaFin = datos.FechaFin;
        destino.Abierta = datos.Abierta;
    }

    private static ConvocatoriaDto ADto(Convocatoria c) => new()
    {
        Id = c.Id,
        Codigo = c.Codigo,
        Titulo = c.Titulo,
        Ejercicio = c.Ejercicio,
        PorcentajeCofinanciacion = c.PorcentajeCofinanciacion,
        FechaInicio = c.FechaInicio,
        FechaFin = c.FechaFin,
        Abierta = c.Abierta
    };
}

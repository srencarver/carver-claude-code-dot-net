using Ayudas.Application.Abstracciones;
using Ayudas.Application.Dtos;
using Ayudas.Application.Excepciones;
using Ayudas.Domain.Comun;

namespace Ayudas.Application.Servicios;

public class EntidadService : IEntidadService
{
    private readonly IEntidadRepository _entidades;

    public EntidadService(IEntidadRepository entidades)
    {
        _entidades = entidades;
    }

    public async Task<List<EntidadListadoDto>> ListarAsync(CancellationToken ct = default)
    {
        var entidades = await _entidades.ListarAsync(ct);

        return entidades
            .Select(e => new EntidadListadoDto
            {
                Id = e.Id,
                Nif = e.Nif,
                Nombre = e.Nombre,
                Tipo = e.Tipo,
                Provincia = e.Provincia,
                Activa = e.Activa
            })
            .ToList();
    }

    public async Task<EntidadEdicionDto?> ObtenerParaEdicionAsync(int id, CancellationToken ct = default)
    {
        var entidad = await _entidades.ObtenerAsync(id, ct);
        if (entidad is null)
        {
            return null;
        }

        return new EntidadEdicionDto
        {
            Id = entidad.Id,
            Nif = entidad.Nif,
            Nombre = entidad.Nombre,
            Tipo = entidad.Tipo,
            CodigoMunicipio = entidad.CodigoMunicipio,
            Provincia = entidad.Provincia,
            DireccionNotificacion = entidad.DireccionNotificacion,
            CorreoNotificacion = entidad.CorreoNotificacion,
            Activa = entidad.Activa,
            FechaModificacion = entidad.FechaModificacion,
            UsuarioModificacion = entidad.UsuarioModificacion
        };
    }

    public async Task ActualizarAsync(EntidadEdicionDto datos, CancellationToken ct = default)
    {
        var entidad = await _entidades.ObtenerAsync(datos.Id, ct)
            ?? throw new EntidadNoEncontradaException(datos.Id);

        var nif = LimpiarNif(datos.Nif);
        if (!Nif.EsValido(nif))
        {
            throw new ValidacionException(nameof(datos.Nif), "El NIF no es válido.");
        }

        var otra = await _entidades.ObtenerPorNifAsync(nif, ct);
        if (otra is not null && otra.Id != entidad.Id)
        {
            throw new ValidacionException(nameof(datos.Nif), "Ya existe otra entidad local con ese NIF.");
        }

        if (datos.CodigoMunicipio.Length != 5 || !datos.CodigoMunicipio.All(char.IsDigit))
        {
            throw new ValidacionException(nameof(datos.CodigoMunicipio), "El código de municipio debe tener 5 dígitos (código INE).");
        }

        entidad.Nif = nif;
        entidad.Nombre = datos.Nombre.Trim();
        entidad.Tipo = datos.Tipo;
        entidad.CodigoMunicipio = datos.CodigoMunicipio;
        entidad.Provincia = datos.Provincia.Trim();
        entidad.DireccionNotificacion = string.IsNullOrWhiteSpace(datos.DireccionNotificacion) ? null : datos.DireccionNotificacion.Trim();
        entidad.CorreoNotificacion = string.IsNullOrWhiteSpace(datos.CorreoNotificacion) ? null : datos.CorreoNotificacion.Trim();
        entidad.Activa = datos.Activa;

        await _entidades.ActualizarAsync(entidad, ct);
    }

    private static string LimpiarNif(string nif) =>
        new string(nif.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
}

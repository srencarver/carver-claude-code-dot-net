using Ayudas.Application.Dtos;
using Ayudas.Web.Models;

namespace Ayudas.Web.Mapeos;

/// <summary>
/// Conversión entre el modelo de la vista y el DTO del servicio.
/// </summary>
public static class EntidadMapeos
{
    public static EntidadEdicionViewModel AViewModel(this EntidadEdicionDto dto) => new()
    {
        Id = dto.Id,
        Nif = dto.Nif,
        Nombre = dto.Nombre,
        Tipo = dto.Tipo,
        CodigoMunicipio = dto.CodigoMunicipio,
        Provincia = dto.Provincia,
        DireccionNotificacion = dto.DireccionNotificacion,
        CorreoNotificacion = dto.CorreoNotificacion,
        Activa = dto.Activa,
        FechaModificacion = dto.FechaModificacion,
        UsuarioModificacion = dto.UsuarioModificacion
    };

    public static EntidadEdicionDto ADto(this EntidadEdicionViewModel modelo) => new()
    {
        Id = modelo.Id,
        Nif = modelo.Nif,
        Nombre = modelo.Nombre,
        Tipo = modelo.Tipo,
        // En el formulario se admite el código con espacios o puntos: se deja solo lo numérico.
        CodigoMunicipio = new string(modelo.CodigoMunicipio.Where(char.IsDigit).ToArray()),
        Provincia = modelo.Provincia,
        DireccionNotificacion = modelo.DireccionNotificacion,
        CorreoNotificacion = modelo.CorreoNotificacion,
        Activa = modelo.Activa
    };
}

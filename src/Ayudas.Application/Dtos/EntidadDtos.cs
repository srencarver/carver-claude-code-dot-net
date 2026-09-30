using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Dtos;

public class EntidadListadoDto
{
    public int Id { get; set; }
    public string Nif { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public TipoEntidad Tipo { get; set; }
    public string Provincia { get; set; } = string.Empty;
    public bool Activa { get; set; }
}

public class EntidadEdicionDto
{
    public int Id { get; set; }
    public string Nif { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public TipoEntidad Tipo { get; set; }
    public string CodigoMunicipio { get; set; } = string.Empty;
    public string Provincia { get; set; } = string.Empty;
    public string? DireccionNotificacion { get; set; }
    public string? CorreoNotificacion { get; set; }
    public bool Activa { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public string? UsuarioModificacion { get; set; }
}

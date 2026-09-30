using System.ComponentModel.DataAnnotations;
using Ayudas.Domain.Entidades;

namespace Ayudas.Web.Models;

public class EntidadEdicionViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El NIF es obligatorio.")]
    [StringLength(12)]
    [Display(Name = "NIF")]
    public string Nif { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(200)]
    public string Nombre { get; set; } = string.Empty;

    public TipoEntidad Tipo { get; set; }

    [Required(ErrorMessage = "El código de municipio es obligatorio.")]
    [Display(Name = "Código de municipio (INE)")]
    public string CodigoMunicipio { get; set; } = string.Empty;

    [Required(ErrorMessage = "La provincia es obligatoria.")]
    [StringLength(60)]
    public string Provincia { get; set; } = string.Empty;

    [StringLength(300)]
    [Display(Name = "Dirección de notificación")]
    public string? DireccionNotificacion { get; set; }

    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [Display(Name = "Correo de notificación")]
    public string? CorreoNotificacion { get; set; }

    public bool Activa { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string? UsuarioModificacion { get; set; }
}

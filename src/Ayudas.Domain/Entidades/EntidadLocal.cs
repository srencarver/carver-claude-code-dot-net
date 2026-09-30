namespace Ayudas.Domain.Entidades;

/// <summary>
/// Entidad local (ayuntamiento, diputación, comarca...) que envía remesas de solicitudes de ayuda.
/// </summary>
public class EntidadLocal
{
    public int Id { get; set; }

    /// <summary>NIF de la entidad (CIF de corporación local: letra P + 7 dígitos + control).</summary>
    public string Nif { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public TipoEntidad Tipo { get; set; }

    /// <summary>Código INE del municipio (5 dígitos).</summary>
    public string CodigoMunicipio { get; set; } = string.Empty;

    public string Provincia { get; set; } = string.Empty;

    /// <summary>Dirección postal para notificaciones. Puede no estar informada.</summary>
    public string? DireccionNotificacion { get; set; }

    public string? CorreoNotificacion { get; set; }

    public bool Activa { get; set; } = true;

    public DateTime FechaAlta { get; set; }

    /// <summary>La rellena el interceptor de auditoría al guardar.</summary>
    public DateTime? FechaModificacion { get; set; }

    /// <summary>La rellena el interceptor de auditoría al guardar.</summary>
    public string? UsuarioModificacion { get; set; }

    public List<Remesa> Remesas { get; set; } = new();
}

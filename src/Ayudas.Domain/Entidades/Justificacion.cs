namespace Ayudas.Domain.Entidades;

/// <summary>
/// Justificación del gasto de una ayuda concedida.
/// Tabla heredada JUSTIFICACIONES: la gestiona el módulo Legacy/Justificaciones de Ayudas.Web.
/// </summary>
public class Justificacion
{
    public int Id { get; set; }

    public int AyudaId { get; set; }
    public Ayuda Ayuda { get; set; } = null!;

    public DateTime FechaPresentacion { get; set; }

    public decimal ImporteJustificado { get; set; }

    /// <summary>Código heredado: P = pendiente, A = aprobada, R = rechazada.</summary>
    public string CodigoEstado { get; set; } = "P";

    public string? Observaciones { get; set; }
}

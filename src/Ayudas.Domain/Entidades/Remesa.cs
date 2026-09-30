namespace Ayudas.Domain.Entidades;

/// <summary>
/// Envío de solicitudes de ayuda que hace una entidad local para una convocatoria.
/// </summary>
public class Remesa
{
    public int Id { get; set; }

    /// <summary>Referencia única de la remesa, por ejemplo REM-2026-000123.</summary>
    public string Referencia { get; set; } = string.Empty;

    public int EntidadLocalId { get; set; }
    public EntidadLocal EntidadLocal { get; set; } = null!;

    public int ConvocatoriaId { get; set; }
    public Convocatoria Convocatoria { get; set; } = null!;

    public DateTime FechaEnvio { get; set; }

    public EstadoRemesa Estado { get; set; } = EstadoRemesa.Recibida;

    public string? Observaciones { get; set; }

    public List<Ayuda> Ayudas { get; set; } = new();
}

namespace Ayudas.Domain.Entidades;

/// <summary>
/// Convocatoria de ayudas a la que se acogen las remesas.
/// </summary>
public class Convocatoria
{
    public int Id { get; set; }

    /// <summary>Código público de la convocatoria, por ejemplo CONV-2026-01.</summary>
    public string Codigo { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public int Ejercicio { get; set; }

    /// <summary>Parte del importe concedido que cofinancia el Estado (0,80 = 80 %).</summary>
    public decimal PorcentajeCofinanciacion { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public bool Abierta { get; set; }

    public List<Remesa> Remesas { get; set; } = new();
}

namespace Ayudas.Application.Dtos;

public class ConvocatoriaDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public int Ejercicio { get; set; }
    public decimal PorcentajeCofinanciacion { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }
    public bool Abierta { get; set; }
}

using System.ComponentModel.DataAnnotations;
using Ayudas.Application.Dtos;

namespace Ayudas.Web.Models;

public class ConvocatoriaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(20)]
    [Display(Name = "Código")]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(250)]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Range(2000, 2100, ErrorMessage = "El ejercicio no es válido.")]
    public int Ejercicio { get; set; } = DateTime.Today.Year;

    [Range(1, 100, ErrorMessage = "El porcentaje debe estar entre 1 y 100.")]
    [Display(Name = "Cofinanciación (%)")]
    public int PorcentajeCofinanciacion { get; set; } = 80;

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de inicio")]
    public DateTime FechaInicio { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de fin")]
    public DateTime FechaFin { get; set; } = DateTime.Today.AddMonths(6);

    public bool Abierta { get; set; } = true;

    public static ConvocatoriaViewModel DesdeDto(ConvocatoriaDto dto) => new()
    {
        Id = dto.Id,
        Codigo = dto.Codigo,
        Titulo = dto.Titulo,
        Ejercicio = dto.Ejercicio,
        PorcentajeCofinanciacion = (int)Math.Round(dto.PorcentajeCofinanciacion * 100),
        FechaInicio = dto.FechaInicio.ToDateTime(TimeOnly.MinValue),
        FechaFin = dto.FechaFin.ToDateTime(TimeOnly.MinValue),
        Abierta = dto.Abierta
    };

    public ConvocatoriaDto ADto() => new()
    {
        Id = Id,
        Codigo = Codigo,
        Titulo = Titulo,
        Ejercicio = Ejercicio,
        PorcentajeCofinanciacion = PorcentajeCofinanciacion / 100m,
        FechaInicio = DateOnly.FromDateTime(FechaInicio),
        FechaFin = DateOnly.FromDateTime(FechaFin),
        Abierta = Abierta
    };
}

namespace Ayudas.Application.Dtos;

public class AyudaListadoDto
{
    public int Id { get; set; }
    public string RemesaReferencia { get; set; } = string.Empty;
    public string NifBeneficiario { get; set; } = string.Empty;
    public string NombreBeneficiario { get; set; } = string.Empty;
    public string CodigoMunicipio { get; set; } = string.Empty;
    public decimal ImporteConcedido { get; set; }
    public DateOnly FechaSolicitud { get; set; }
}

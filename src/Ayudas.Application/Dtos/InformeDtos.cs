namespace Ayudas.Application.Dtos;

public class ResumenConvocatoriaDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public int NumeroRemesas { get; set; }
    public decimal ImporteConcedido { get; set; }
}

public class JustificacionPendienteDto
{
    public int IdJustificacion { get; set; }
    public string ReferenciaRemesa { get; set; } = string.Empty;
    public string NombreBeneficiario { get; set; } = string.Empty;
    public DateTime FechaPresentacion { get; set; }
    public decimal ImporteJustificado { get; set; }
}

public class InformeResumenDto
{
    public List<ResumenConvocatoriaDto> Convocatorias { get; set; } = new();
    public List<JustificacionPendienteDto> JustificacionesPendientes { get; set; } = new();
}

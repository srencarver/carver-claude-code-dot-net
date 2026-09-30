namespace Ayudas.Web.Legacy.Justificaciones
{
    public class JustificacionFila
    {
        public int IdJustificacion { get; set; }
        public int IdAyuda { get; set; }
        public DateTime FechaPresentacion { get; set; }
        public decimal ImporteJustificado { get; set; }
        public decimal ImporteConcedido { get; set; }
        public string Estado { get; set; } = "";
        public string Observaciones { get; set; } = "";
        public string NifBeneficiario { get; set; } = "";
        public string NombreBeneficiario { get; set; } = "";

        public string DescripcionEstado
        {
            get
            {
                if (Estado == "P") return "Pendiente";
                if (Estado == "A") return "Aprobada";
                if (Estado == "R") return "Rechazada";
                return Estado;
            }
        }
    }
}

using Ayudas.Domain.Entidades;
using Ayudas.Importacion.Validacion;

namespace Ayudas.Importacion.Mapeo;

public static class AyudaMapper
{
    /// <summary>Ayuda nueva a partir de una solicitud válida. El importe concedido se fija al resolver.</summary>
    public static Ayuda ANueva(AyudaNormalizada solicitud) => new()
    {
        NifBeneficiario = solicitud.NifBeneficiario,
        NombreBeneficiario = solicitud.NombreBeneficiario,
        CodigoMunicipio = solicitud.CodigoMunicipio,
        Concepto = solicitud.Concepto,
        ImporteSolicitado = solicitud.ImporteSolicitado,
        ImporteConcedido = 0m,
        FechaSolicitud = solicitud.FechaSolicitud
    };
}

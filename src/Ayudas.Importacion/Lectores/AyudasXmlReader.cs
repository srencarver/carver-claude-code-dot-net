using System.Xml.Linq;
using Ayudas.Importacion.Modelos;

namespace Ayudas.Importacion.Lectores;

/// <summary>
/// Lector del formato del contrato: docs/ayudas.xsd, versión 1.0 (raíz RemesaAyudas).
/// </summary>
public class AyudasXmlReader : IAyudasLector
{
    private static readonly HashSet<string> CamposSolicitud = new()
    {
        "NifBeneficiario", "NombreBeneficiario", "CodigoMunicipio", "Concepto", "ImporteSolicitado", "FechaSolicitud"
    };

    public string Formato => "RemesaAyudas 1.0";

    public bool Soporta(XDocument documento) => documento.Root?.Name.LocalName == "RemesaAyudas";

    public LecturaRemesa Leer(XDocument documento)
    {
        var raiz = documento.Root ?? throw new FormatException("El fichero no tiene elemento raíz.");
        var cabecera = raiz.Element("Cabecera") ?? throw new FormatException("Falta la cabecera de la remesa.");

        var lectura = new LecturaRemesa
        {
            Formato = Formato,
            Cabecera = new CabeceraRemesa
            {
                Referencia = Texto(cabecera, "Referencia"),
                NifEntidad = Texto(cabecera, "NifEntidad"),
                CodigoConvocatoria = Texto(cabecera, "CodigoConvocatoria"),
                FechaEnvio = Texto(cabecera, "FechaEnvio"),
                NumeroSolicitudes = Texto(cabecera, "NumeroSolicitudes"),
                ImporteTotal = Texto(cabecera, "ImporteTotal")
            }
        };

        var solicitudes = raiz.Element("Solicitudes")?.Elements("Solicitud") ?? Enumerable.Empty<XElement>();
        var fila = 0;
        foreach (var solicitud in solicitudes)
        {
            fila++;
            lectura.Solicitudes.Add(new AyudaEntrada
            {
                Fila = fila,
                NifBeneficiario = Texto(solicitud, "NifBeneficiario"),
                NombreBeneficiario = Texto(solicitud, "NombreBeneficiario"),
                CodigoMunicipio = Texto(solicitud, "CodigoMunicipio"),
                Concepto = Texto(solicitud, "Concepto"),
                ImporteSolicitado = Texto(solicitud, "ImporteSolicitado"),
                FechaSolicitud = Texto(solicitud, "FechaSolicitud"),
                ElementosDesconocidos = solicitud.Elements()
                    .Select(e => e.Name.LocalName)
                    .Where(nombre => !CamposSolicitud.Contains(nombre))
                    .ToList()
            });
        }

        return lectura;
    }

    private static string? Texto(XElement padre, string nombre) => padre.Element(nombre)?.Value;
}

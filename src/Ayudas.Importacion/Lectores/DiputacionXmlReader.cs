using System.Xml.Linq;
using Ayudas.Importacion.Modelos;

namespace Ayudas.Importacion.Lectores;

/// <summary>
/// Lector del formato propio de la Diputación Provincial de León (raíz EnvioDiputacion).
/// Los datos de la remesa van en atributos del elemento raíz y cada solicitud es un Beneficiario.
/// No trae importe total.
/// </summary>
public class DiputacionXmlReader : IAyudasLector
{
    private static readonly HashSet<string> CamposBeneficiario = new() { "Nombre", "Actuacion", "Importe", "Fecha" };

    public string Formato => "EnvioDiputacion (Diputación de León)";

    public bool Soporta(XDocument documento) => documento.Root?.Name.LocalName == "EnvioDiputacion";

    public LecturaRemesa Leer(XDocument documento)
    {
        var raiz = documento.Root ?? throw new FormatException("El fichero no tiene elemento raíz.");
        var beneficiarios = raiz.Elements("Beneficiario").ToList();

        var lectura = new LecturaRemesa
        {
            Formato = Formato,
            Cabecera = new CabeceraRemesa
            {
                Referencia = raiz.Attribute("referencia")?.Value,
                NifEntidad = raiz.Attribute("diputacion")?.Value,
                CodigoConvocatoria = raiz.Attribute("convocatoria")?.Value,
                FechaEnvio = raiz.Attribute("fecha")?.Value,
                NumeroSolicitudes = beneficiarios.Count.ToString(),
                ImporteTotal = null
            }
        };

        var fila = 0;
        foreach (var beneficiario in beneficiarios)
        {
            fila++;
            lectura.Solicitudes.Add(new AyudaEntrada
            {
                Fila = fila,
                NifBeneficiario = beneficiario.Attribute("nif")?.Value,
                CodigoMunicipio = beneficiario.Attribute("municipio")?.Value,
                NombreBeneficiario = beneficiario.Element("Nombre")?.Value,
                Concepto = beneficiario.Element("Actuacion")?.Value,
                ImporteSolicitado = beneficiario.Element("Importe")?.Value,
                FechaSolicitud = beneficiario.Element("Fecha")?.Value,
                ElementosDesconocidos = beneficiario.Elements()
                    .Select(e => e.Name.LocalName)
                    .Where(nombre => !CamposBeneficiario.Contains(nombre))
                    .ToList()
            });
        }

        return lectura;
    }
}

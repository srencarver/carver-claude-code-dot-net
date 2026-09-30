using System.Xml.Linq;
using Ayudas.Importacion.Modelos;

namespace Ayudas.Importacion.Lectores;

/// <summary>
/// Lector de un formato de fichero de remesa. Hay uno por formato que envían las entidades.
/// </summary>
public interface IAyudasLector
{
    /// <summary>Nombre del formato, para informes y trazas.</summary>
    string Formato { get; }

    /// <summary>Indica si el documento tiene el formato que entiende este lector.</summary>
    bool Soporta(XDocument documento);

    /// <summary>Lee la cabecera y las solicitudes sin validarlas.</summary>
    LecturaRemesa Leer(XDocument documento);
}

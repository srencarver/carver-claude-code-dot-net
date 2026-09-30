using Ayudas.Importacion;

namespace Ayudas.Web.Models;

public class ImportacionViewModel
{
    public string NombreFichero { get; set; } = string.Empty;

    public ResultadoImportacion Resultado { get; set; } = null!;

    public string? NumeroRegistro { get; set; }

    public string? AvisoRegistro { get; set; }
}

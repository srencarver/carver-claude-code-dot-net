namespace Ayudas.Domain.Entidades;

/// <summary>
/// Catálogo de municipios (código INE).
/// </summary>
public class Municipio
{
    /// <summary>Código INE de 5 dígitos: 2 de provincia + 3 de municipio.</summary>
    public string CodigoIne { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Provincia { get; set; } = string.Empty;
}

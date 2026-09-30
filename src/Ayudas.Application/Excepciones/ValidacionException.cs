namespace Ayudas.Application.Excepciones;

/// <summary>
/// Error de validación de negocio. El controlador lo traslada al ModelState.
/// </summary>
public class ValidacionException : Exception
{
    public ValidacionException(string campo, string mensaje) : base(mensaje)
    {
        Campo = campo;
    }

    public string Campo { get; }
}

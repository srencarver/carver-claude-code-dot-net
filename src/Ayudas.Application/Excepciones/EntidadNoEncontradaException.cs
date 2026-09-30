namespace Ayudas.Application.Excepciones;

public class EntidadNoEncontradaException : Exception
{
    public EntidadNoEncontradaException(int id) : base($"No existe la entidad local {id}.")
    {
        Id = id;
    }

    public int Id { get; }
}

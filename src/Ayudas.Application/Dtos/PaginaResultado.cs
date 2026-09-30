namespace Ayudas.Application.Dtos;

public class PaginaResultado<T>
{
    public PaginaResultado(List<T> elementos, int pagina, int tamanoPagina, int totalElementos)
    {
        Elementos = elementos;
        Pagina = pagina;
        TamanoPagina = tamanoPagina;
        TotalElementos = totalElementos;
    }

    public List<T> Elementos { get; }

    public int Pagina { get; }

    public int TamanoPagina { get; }

    public int TotalElementos { get; }

    public int TotalPaginas => TamanoPagina == 0 ? 0 : (int)Math.Ceiling(TotalElementos / (double)TamanoPagina);
}

using Ayudas.Application.Dtos;
using Ayudas.Domain.Entidades;

namespace Ayudas.Web.Models;

public class RemesasIndexViewModel
{
    public PaginaResultado<RemesaListadoDto> Resultado { get; set; } = null!;

    public int? EntidadId { get; set; }

    public EstadoRemesa? Estado { get; set; }

    public List<EntidadListadoDto> Entidades { get; set; } = new();
}

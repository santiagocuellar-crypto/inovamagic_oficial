using MediatR;
using Microsoft.EntityFrameworkCore;
using ProyectoGrado.Application.Common.Interfaces;

namespace ProyectoGrado.Application.Features.Pedidos.Queries.ListarPedidos;

public class ListarPedidosQueryHandler : IRequestHandler<ListarPedidosQuery, List<PedidoDto>>
{
    private readonly IApplicationDbContext _context;

    public ListarPedidosQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PedidoDto>> Handle(ListarPedidosQuery request, CancellationToken cancellationToken)
    {
        return await _context.Pedidos
            .Include(p => p.Items)
            .OrderByDescending(p => p.FechaCreacion)
            .Select(p => new PedidoDto(
                p.Id,
                p.ClienteNombre,
                p.ClienteTelefono,
                p.ClienteEmail,
                p.DireccionCalle,
                p.DireccionCiudad,
                p.DireccionNotas,
                p.Estado.ToString(),
                p.Total,
                p.FechaCreacion,
                p.Items.Select(i => new ItemPedidoDto(
                    i.ProductoNombre, i.Talla, i.Cantidad, i.PrecioUnitario)).ToList()))
            .ToListAsync(cancellationToken);
    }
}

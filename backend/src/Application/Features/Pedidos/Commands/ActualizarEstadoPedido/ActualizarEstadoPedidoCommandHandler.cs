using MediatR;
using Microsoft.EntityFrameworkCore;
using ProyectoGrado.Application.Common.Interfaces;

namespace ProyectoGrado.Application.Features.Pedidos.Commands.ActualizarEstadoPedido;

public class ActualizarEstadoPedidoCommandHandler : IRequestHandler<ActualizarEstadoPedidoCommand>
{
    private readonly IApplicationDbContext _context;

    public ActualizarEstadoPedidoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActualizarEstadoPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.Id == request.PedidoId, cancellationToken)
            ?? throw new InvalidOperationException("Pedido no encontrado");

        pedido.Estado = request.NuevoEstado;
        await _context.SaveChangesAsync(cancellationToken);
    }
}

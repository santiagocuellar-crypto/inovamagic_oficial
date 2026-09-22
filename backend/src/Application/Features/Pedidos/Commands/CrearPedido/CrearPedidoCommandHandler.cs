using MediatR;
using Microsoft.EntityFrameworkCore;
using ProyectoGrado.Application.Common.Interfaces;
using ProyectoGrado.Domain.Entities;

namespace ProyectoGrado.Application.Features.Pedidos.Commands.CrearPedido;

public class CrearPedidoCommandHandler : IRequestHandler<CrearPedidoCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CrearPedidoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CrearPedidoCommand request, CancellationToken cancellationToken)
    {
        if (request.Items.Count == 0)
            throw new InvalidOperationException("El pedido debe tener al menos un producto");

        var pedido = new Pedido
        {
            TenantId = request.TenantId,
            ClienteNombre = request.ClienteNombre,
            ClienteTelefono = request.ClienteTelefono,
            ClienteEmail = request.ClienteEmail,
            DireccionCalle = request.DireccionCalle,
            DireccionCiudad = request.DireccionCiudad,
            DireccionNotas = request.DireccionNotas,
            Estado = EstadoPedido.Pendiente
        };

        decimal total = 0;

        foreach (var itemInput in request.Items)
        {
            var producto = await _context.Productos
                .IgnoreQueryFilters()
                .Include(p => p.Variantes)
                .FirstOrDefaultAsync(
                    p => p.Id == itemInput.ProductoId && p.TenantId == request.TenantId && p.Activo,
                    cancellationToken)
                ?? throw new InvalidOperationException("Producto no encontrado o no disponible");

            var variante = producto.Variantes.FirstOrDefault(v => v.Talla == itemInput.Talla)
                ?? throw new InvalidOperationException($"La talla '{itemInput.Talla}' no existe para '{producto.Nombre}'");

            if (variante.Stock < itemInput.Cantidad)
                throw new InvalidOperationException(
                    $"Stock insuficiente para '{producto.Nombre}' talla {itemInput.Talla}. Disponible: {variante.Stock}");

            variante.Stock -= itemInput.Cantidad;

            var subtotal = producto.Precio * itemInput.Cantidad;
            total += subtotal;

            pedido.Items.Add(new PedidoItem
            {
                ProductoId = producto.Id,
                ProductoNombre = producto.Nombre,
                Talla = itemInput.Talla,
                Cantidad = itemInput.Cantidad,
                PrecioUnitario = producto.Precio
            });
        }

        pedido.Total = total;

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync(cancellationToken);

        return pedido.Id;
    }
}

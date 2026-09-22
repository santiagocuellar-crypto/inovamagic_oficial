namespace ProyectoGrado.Application.Features.Pedidos.Commands.CrearPedido;

public record ItemPedidoInputDto(Guid ProductoId, string Talla, int Cantidad);

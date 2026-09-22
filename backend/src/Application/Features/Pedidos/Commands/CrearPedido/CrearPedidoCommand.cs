using MediatR;

namespace ProyectoGrado.Application.Features.Pedidos.Commands.CrearPedido;

public record CrearPedidoCommand(
    Guid TenantId,
    string ClienteNombre,
    string ClienteTelefono,
    string? ClienteEmail,
    string DireccionCalle,
    string DireccionCiudad,
    string? DireccionNotas,
    List<ItemPedidoInputDto> Items) : IRequest<Guid>;

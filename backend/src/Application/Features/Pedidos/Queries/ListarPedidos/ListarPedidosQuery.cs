using MediatR;

namespace ProyectoGrado.Application.Features.Pedidos.Queries.ListarPedidos;

public record ItemPedidoDto(string ProductoNombre, string Talla, int Cantidad, decimal PrecioUnitario);

public record PedidoDto(
    Guid Id,
    string ClienteNombre,
    string ClienteTelefono,
    string? ClienteEmail,
    string DireccionCalle,
    string DireccionCiudad,
    string? DireccionNotas,
    string Estado,
    decimal Total,
    DateTime FechaCreacion,
    List<ItemPedidoDto> Items);

public record ListarPedidosQuery : IRequest<List<PedidoDto>>;

using MediatR;

namespace ProyectoGrado.Application.Features.Productos.Queries.ListarProductos;

public record ListarProductosQuery(Guid TenantId) : IRequest<List<ProductoListadoDto>>;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ProyectoGrado.Application.Common.Interfaces;

namespace ProyectoGrado.Application.Features.Productos.Queries.ListarProductos;

public class ListarProductosQueryHandler
    : IRequestHandler<ListarProductosQuery, List<ProductoListadoDto>>
{
    private readonly IApplicationDbContext _context;

    public ListarProductosQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductoListadoDto>> Handle(
        ListarProductosQuery request, CancellationToken cancellationToken)
    {
        var productos = await _context.Productos
            .IgnoreQueryFilters()
            .Where(p => p.TenantId == request.TenantId && p.Activo)
            .Include(p => p.Categoria)
            .Include(p => p.Imagenes)
            .Include(p => p.Variantes)
            .Select(p => new ProductoListadoDto(
                p.Id,
                p.Nombre,
                p.Descripcion,
                p.Precio,
                p.Categoria != null ? p.Categoria.Nombre : null,
                p.Imagenes.OrderBy(i => i.Orden).Select(i => i.Url).ToList(),
                p.Variantes.Select(v => new VarianteDto(v.Talla, v.Stock)).ToList()))
            .ToListAsync(cancellationToken);

        return productos;
    }
}

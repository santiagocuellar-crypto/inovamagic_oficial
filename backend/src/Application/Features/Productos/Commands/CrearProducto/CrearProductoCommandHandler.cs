using MediatR;
using ProyectoGrado.Application.Common.Interfaces;
using ProyectoGrado.Domain.Entities;

namespace ProyectoGrado.Application.Features.Productos.Commands.CrearProducto;

public class CrearProductoCommandHandler : IRequestHandler<CrearProductoCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentTenantService _currentTenantService;

    public CrearProductoCommandHandler(
        IApplicationDbContext context, ICurrentTenantService currentTenantService)
    {
        _context = context;
        _currentTenantService = currentTenantService;
    }

    public async Task<Guid> Handle(CrearProductoCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentTenantService.TenantId
            ?? throw new InvalidOperationException("No hay un tenant identificado para este usuario");

        var producto = new Producto
        {
            TenantId = tenantId,
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Precio = request.Precio,
            CategoriaId = request.CategoriaId
        };

        var variantes = request.Variantes.Count > 0
            ? request.Variantes
            : new List<VarianteInputDto> { new("Unica", 0) };

        foreach (var v in variantes)
        {
            producto.Variantes.Add(new ProductoVariante
            {
                Talla = v.Talla,
                Stock = v.Stock
            });
        }

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync(cancellationToken);

        return producto.Id;
    }
}

using Microsoft.EntityFrameworkCore;
using ProyectoGrado.Domain.Entities;

namespace ProyectoGrado.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<Categoria> Categorias { get; }
    DbSet<Producto> Productos { get; }
    DbSet<ProductoVariante> ProductoVariantes { get; }
    DbSet<ProductoImagen> ProductoImagenes { get; }
    DbSet<Pedido> Pedidos { get; }
    DbSet<PedidoItem> PedidoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

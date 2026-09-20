using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoGrado.Application.Common.Interfaces;
using ProyectoGrado.Domain.Common;
using ProyectoGrado.Domain.Entities;
using ProyectoGrado.Infrastructure.Identity;

namespace ProyectoGrado.Infrastructure.Persistence;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly ICurrentTenantService _currentTenantService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentTenantService currentTenantService) : base(options)
    {
        _currentTenantService = currentTenantService;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();

    // A partir de aqui se van agregando los DbSet de futuros modulos
    // (Productos, Pedidos, etc.), y como implementaran ITenantEntity,
    // el filtro de abajo los protege automaticamente.

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Por cada entidad que implemente ITenantEntity, se agrega
        // automaticamente un filtro "WHERE TenantId = tenantActual".
        // Esto evita que alguien se olvide de filtrar a mano en una query
        // y termine mostrando datos de otro negocio por error.
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(GetTenantFilter),
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                    .MakeGenericMethod(entityType.ClrType);

                var filter = method.Invoke(this, null);
                entityType.SetQueryFilter((System.Linq.Expressions.LambdaExpression)filter!);
            }
        }
    }

    private System.Linq.Expressions.LambdaExpression GetTenantFilter<TEntity>()
        where TEntity : class, ITenantEntity
    {
        System.Linq.Expressions.Expression<Func<TEntity, bool>> filter =
            e => e.TenantId == (_currentTenantService.TenantId ?? Guid.Empty);
        return filter;
    }
}

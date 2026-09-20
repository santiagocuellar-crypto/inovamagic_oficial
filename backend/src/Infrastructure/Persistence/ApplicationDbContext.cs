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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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

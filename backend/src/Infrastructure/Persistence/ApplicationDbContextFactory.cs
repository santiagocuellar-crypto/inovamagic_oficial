using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProyectoGrado.Application.Common.Interfaces;

namespace ProyectoGrado.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=ProyectoGradoDb;User Id=sa;Password=ProyectoGrado#2026;TrustServerCertificate=True;");

        return new ApplicationDbContext(optionsBuilder.Options, new DesignTimeTenantService());
    }

    private class DesignTimeTenantService : ICurrentTenantService
    {
        public Guid? TenantId => null;
    }
}

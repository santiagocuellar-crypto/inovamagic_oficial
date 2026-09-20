using Microsoft.AspNetCore.Identity;

namespace ProyectoGrado.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() : base() { }
    public ApplicationRole(string roleName) : base(roleName) { }
}

// Roles fijos que usaremos en toda la plataforma
public static class Roles
{
    public const string Owner = "Owner";
    public const string Admin = "Admin";
    public const string Vendedor = "Vendedor";
}

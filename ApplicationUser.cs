using Microsoft.AspNetCore.Identity;

namespace ProyectoGrado.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid TenantId { get; set; }
}

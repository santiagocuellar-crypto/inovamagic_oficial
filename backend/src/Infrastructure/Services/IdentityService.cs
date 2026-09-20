using Microsoft.AspNetCore.Identity;
using ProyectoGrado.Application.Common.Interfaces;
using ProyectoGrado.Domain.Entities;
using ProyectoGrado.Infrastructure.Identity;
using ProyectoGrado.Infrastructure.Persistence;

namespace ProyectoGrado.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ResultadoAuth> RegistrarNuevoTenantAsync(
        string nombreTenant, string email, string password)
    {
        var usuarioExistente = await _userManager.FindByEmailAsync(email);
        if (usuarioExistente is not null)
            return new ResultadoAuth(false, null, "Ya existe una cuenta con ese correo");

        var tenant = new Tenant
        {
            Nombre = nombreTenant,
            Slug = GenerarSlug(nombreTenant)
        };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        var usuario = new ApplicationUser
        {
            UserName = email,
            Email = email,
            TenantId = tenant.Id
        };

        var resultado = await _userManager.CreateAsync(usuario, password);
        if (!resultado.Succeeded)
        {
            var errores = string.Join(", ", resultado.Errors.Select(e => e.Description));
            return new ResultadoAuth(false, null, errores);
        }

        if (!await _roleManager.RoleExistsAsync(Roles.Owner))
            await _roleManager.CreateAsync(new ApplicationRole(Roles.Owner));

        await _userManager.AddToRoleAsync(usuario, Roles.Owner);

        var token = _jwtTokenGenerator.GenerarToken(
            usuario.Id, usuario.Email!, tenant.Id, new List<string> { Roles.Owner });

        return new ResultadoAuth(true, token, null);
    }

    public async Task<ResultadoAuth> LoginAsync(string email, string password)
    {
        var usuario = await _userManager.FindByEmailAsync(email);
        if (usuario is null)
            return new ResultadoAuth(false, null, "Correo o contrasena incorrectos");

        var passwordValida = await _userManager.CheckPasswordAsync(usuario, password);
        if (!passwordValida)
            return new ResultadoAuth(false, null, "Correo o contrasena incorrectos");

        var roles = await _userManager.GetRolesAsync(usuario);

        var token = _jwtTokenGenerator.GenerarToken(
            usuario.Id, usuario.Email!, usuario.TenantId, roles);

        return new ResultadoAuth(true, token, null);
    }

    private static string GenerarSlug(string nombre)
    {
        var slug = nombre.Trim().ToLowerInvariant().Replace(" ", "-");
        return $"{slug}-{Guid.NewGuid().ToString()[..6]}";
    }
}

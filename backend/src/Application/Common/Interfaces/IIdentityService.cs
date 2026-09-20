namespace ProyectoGrado.Application.Common.Interfaces;

public record ResultadoAuth(bool Exitoso, string? Token, string? Error);

public interface IIdentityService
{
    Task<ResultadoAuth> RegistrarNuevoTenantAsync(
        string nombreTenant, string email, string password);

    Task<ResultadoAuth> LoginAsync(string email, string password);
}

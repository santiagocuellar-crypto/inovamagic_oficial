namespace ProyectoGrado.Application.Common.Interfaces;

// Infrastructure la implementa leyendo el TenantId desde el token JWT
// del usuario que hizo la peticion actual.
public interface ICurrentTenantService
{
    Guid? TenantId { get; }
}

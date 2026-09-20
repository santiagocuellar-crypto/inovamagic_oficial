namespace ProyectoGrado.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerarToken(Guid usuarioId, string email, Guid tenantId, IList<string> roles);
}

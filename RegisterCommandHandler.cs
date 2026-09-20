using MediatR;
using ProyectoGrado.Application.Common.Interfaces;

namespace ProyectoGrado.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var resultado = await _identityService.RegistrarNuevoTenantAsync(
            request.NombreTenant, request.Email, request.Password);

        return new RegisterResult(resultado.Exitoso, resultado.Token, resultado.Error);
    }
}

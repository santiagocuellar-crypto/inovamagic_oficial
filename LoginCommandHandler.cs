using MediatR;
using ProyectoGrado.Application.Common.Interfaces;

namespace ProyectoGrado.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var resultado = await _identityService.LoginAsync(request.Email, request.Password);
        return new LoginResult(resultado.Exitoso, resultado.Token, resultado.Error);
    }
}

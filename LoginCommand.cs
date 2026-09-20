using MediatR;

namespace ProyectoGrado.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;

public record LoginResult(bool Exitoso, string? Token, string? Error);

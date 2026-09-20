using MediatR;

namespace ProyectoGrado.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string NombreTenant,
    string Email,
    string Password) : IRequest<RegisterResult>;

public record RegisterResult(bool Exitoso, string? Token, string? Error);

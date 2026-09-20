using FluentValidation;

namespace ProyectoGrado.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.NombreTenant)
            .NotEmpty().WithMessage("El nombre del negocio es obligatorio")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es obligatorio")
            .EmailAddress().WithMessage("El correo no tiene un formato valido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contrasena es obligatoria")
            .MinimumLength(8).WithMessage("La contrasena debe tener al menos 8 caracteres");
    }
}

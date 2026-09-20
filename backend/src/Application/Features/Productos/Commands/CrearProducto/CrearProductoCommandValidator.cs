using FluentValidation;

namespace ProyectoGrado.Application.Features.Productos.Commands.CrearProducto;

public class CrearProductoCommandValidator : AbstractValidator<CrearProductoCommand>
{
    public CrearProductoCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio")
            .MaximumLength(200);

        RuleFor(x => x.Precio)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero");
    }
}

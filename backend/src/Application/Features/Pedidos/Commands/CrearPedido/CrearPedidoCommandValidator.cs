using FluentValidation;

namespace ProyectoGrado.Application.Features.Pedidos.Commands.CrearPedido;

public class CrearPedidoCommandValidator : AbstractValidator<CrearPedidoCommand>
{
    public CrearPedidoCommandValidator()
    {
        RuleFor(x => x.ClienteNombre).NotEmpty().WithMessage("El nombre es obligatorio");
        RuleFor(x => x.ClienteTelefono).NotEmpty().WithMessage("El telefono es obligatorio");
        RuleFor(x => x.DireccionCalle).NotEmpty().WithMessage("La direccion es obligatoria");
        RuleFor(x => x.DireccionCiudad).NotEmpty().WithMessage("La ciudad es obligatoria");
        RuleFor(x => x.Items).NotEmpty().WithMessage("El pedido debe tener al menos un producto");
    }
}

using MediatR;

namespace ProyectoGrado.Application.Features.Productos.Commands.CrearProducto;

public record CrearProductoCommand(
    string Nombre,
    string Descripcion,
    decimal Precio,
    Guid? CategoriaId,
    List<VarianteInputDto> Variantes) : IRequest<Guid>;

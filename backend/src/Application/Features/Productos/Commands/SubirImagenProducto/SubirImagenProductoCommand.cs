using MediatR;

namespace ProyectoGrado.Application.Features.Productos.Commands.SubirImagenProducto;

public record SubirImagenProductoCommand(
    Guid ProductoId,
    Stream ContenidoArchivo,
    string NombreArchivo) : IRequest<string>;

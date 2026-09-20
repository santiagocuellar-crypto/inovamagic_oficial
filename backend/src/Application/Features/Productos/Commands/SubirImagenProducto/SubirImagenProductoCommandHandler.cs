using MediatR;
using Microsoft.EntityFrameworkCore;
using ProyectoGrado.Application.Common.Interfaces;
using ProyectoGrado.Domain.Entities;

namespace ProyectoGrado.Application.Features.Productos.Commands.SubirImagenProducto;

public class SubirImagenProductoCommandHandler
    : IRequestHandler<SubirImagenProductoCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IImageStorageService _imageStorageService;

    public SubirImagenProductoCommandHandler(
        IApplicationDbContext context, IImageStorageService imageStorageService)
    {
        _context = context;
        _imageStorageService = imageStorageService;
    }

    public async Task<string> Handle(SubirImagenProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == request.ProductoId, cancellationToken)
            ?? throw new InvalidOperationException("Producto no encontrado");

        var url = await _imageStorageService.SubirImagenAsync(
            request.ContenidoArchivo, request.NombreArchivo);

        _context.ProductoImagenes.Add(new ProductoImagen
        {
            ProductoId = producto.Id,
            Url = url,
            Orden = 0
        });

        await _context.SaveChangesAsync(cancellationToken);

        return url;
    }
}

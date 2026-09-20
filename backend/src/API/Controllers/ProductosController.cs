using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoGrado.Application.Features.Productos.Commands.CrearProducto;
using ProyectoGrado.Application.Features.Productos.Commands.SubirImagenProducto;
using ProyectoGrado.Application.Features.Productos.Queries.ListarProductos;

namespace ProyectoGrado.API.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Crear(CrearProductoCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }

    [Authorize]
    [HttpPost("{productoId}/imagenes")]
    public async Task<IActionResult> SubirImagen(Guid productoId, IFormFile archivo)
    {
        await using var stream = archivo.OpenReadStream();
        var url = await _mediator.Send(
            new SubirImagenProductoCommand(productoId, stream, archivo.FileName));

        return Ok(new { url });
    }

    [AllowAnonymous]
    [HttpGet("publico/{tenantId}")]
    public async Task<IActionResult> ListarPublico(Guid tenantId)
    {
        var productos = await _mediator.Send(new ListarProductosQuery(tenantId));
        return Ok(productos);
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoGrado.Application.Features.Categorias.Commands.CrearCategoria;
using ProyectoGrado.Application.Features.Categorias.Queries.ListarCategorias;

namespace ProyectoGrado.API.Controllers;

[ApiController]
[Route("api/categorias")]
[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearCategoriaCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var categorias = await _mediator.Send(new ListarCategoriasQuery());
        return Ok(categorias);
    }
}

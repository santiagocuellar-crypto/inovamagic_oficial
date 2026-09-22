using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoGrado.Application.Features.Pedidos.Commands.ActualizarEstadoPedido;
using ProyectoGrado.Application.Features.Pedidos.Commands.CrearPedido;
using ProyectoGrado.Application.Features.Pedidos.Queries.ListarPedidos;

namespace ProyectoGrado.API.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PedidosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Crear(CrearPedidoCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return Ok(new { id });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var pedidos = await _mediator.Send(new ListarPedidosQuery());
        return Ok(pedidos);
    }

    [Authorize]
    [HttpPut("{pedidoId}/estado")]
    public async Task<IActionResult> ActualizarEstado(Guid pedidoId, ActualizarEstadoBodyDto body)
    {
        await _mediator.Send(new ActualizarEstadoPedidoCommand(pedidoId, body.NuevoEstado));
        return NoContent();
    }
}

public record ActualizarEstadoBodyDto(ProyectoGrado.Domain.Entities.EstadoPedido NuevoEstado);

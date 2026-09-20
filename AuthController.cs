using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProyectoGrado.Application.Features.Auth.Commands.Login;
using ProyectoGrado.Application.Features.Auth.Commands.Register;

namespace ProyectoGrado.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var resultado = await _mediator.Send(command);

        if (!resultado.Exitoso)
            return BadRequest(new { error = resultado.Error });

        return Ok(new { token = resultado.Token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var resultado = await _mediator.Send(command);

        if (!resultado.Exitoso)
            return Unauthorized(new { error = resultado.Error });

        return Ok(new { token = resultado.Token });
    }
}

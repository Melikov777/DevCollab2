using DevCollab.Application.DTOs;
using DevCollab.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevCollab.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        var command = new RegisterCommand { RegisterDto = registerDto };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var command = new LoginCommand { LoginDto = loginDto };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

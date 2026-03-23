using System.Security.Claims;
using DevCollab.Application.DTOs;
using DevCollab.Application.Features.Projects.Commands;
using DevCollab.Application.Features.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevCollab.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ProjectDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllProjectsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProjectDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery { ProjectId = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto createProjectDto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new CreateProjectCommand { CreateProjectDto = createProjectDto, UserId = userId };
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new DeleteProjectCommand { ProjectId = id, UserId = userId };
        await _mediator.Send(command);
        return NoContent();
    }
}

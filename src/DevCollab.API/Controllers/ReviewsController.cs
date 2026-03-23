using System.Security.Claims;
using DevCollab.Application.DTOs;
using DevCollab.Application.Features.Reviews.Commands;
using DevCollab.Application.Features.Reviews.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevCollab.API.Controllers;

[ApiController]
[Route("api/projects/{projectId}/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReviewDto>>> GetByProject(Guid projectId)
    {
        var result = await _mediator.Send(new GetReviewsByProjectQuery { ProjectId = projectId });
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ReviewDto>> Create(Guid projectId, [FromBody] CreateReviewDto createReviewDto)
    {
        if (projectId != createReviewDto.ProjectId)
            return BadRequest("Project ID mismatch.");

        var reviewerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new CreateReviewCommand { CreateReviewDto = createReviewDto, ReviewerId = reviewerId };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

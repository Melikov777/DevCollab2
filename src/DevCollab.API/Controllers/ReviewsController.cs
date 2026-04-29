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
    public async Task<ActionResult<PagedResult<ReviewDto>>> GetByProject(Guid projectId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetReviewsByProjectQuery
        {
            ProjectId = projectId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ReviewDto>> Create(Guid projectId, [FromBody] CreateReviewDto createReviewDto)
    {
        if (projectId != createReviewDto.ProjectId)
            return BadRequest("Project ID mismatch.");

        if (!TryGetCurrentUserId(out var reviewerId))
            return Unauthorized("Invalid user token.");

        var command = new CreateReviewCommand { CreateReviewDto = createReviewDto, ReviewerId = reviewerId };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    private bool TryGetCurrentUserId(out Guid userId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(userIdClaim, out userId);
    }
}

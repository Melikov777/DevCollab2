using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Reviews.Queries;

public class GetReviewsByProjectQuery : IRequest<PagedResult<ReviewDto>>
{
    public Guid ProjectId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Reviews.Queries;

public class GetReviewsByProjectQuery : IRequest<List<ReviewDto>>
{
    public Guid ProjectId { get; set; }
}

using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Reviews.Commands;

public class CreateReviewCommand : IRequest<ReviewDto>
{
    public CreateReviewDto CreateReviewDto { get; set; } = null!;
    public Guid ReviewerId { get; set; }
}

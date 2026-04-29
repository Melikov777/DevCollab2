using FluentValidation;

namespace DevCollab.Application.Features.Reviews.Validators;

public class CreateReviewCommandValidator : AbstractValidator<Commands.CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.ReviewerId).NotEmpty();
        RuleFor(x => x.CreateReviewDto.ProjectId).NotEmpty();
        RuleFor(x => x.CreateReviewDto.Content).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.CreateReviewDto.Rating).InclusiveBetween(1, 5);
    }
}

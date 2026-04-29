using FluentValidation;

namespace DevCollab.Application.Features.Reviews.Validators;

public class GetReviewsByProjectQueryValidator : AbstractValidator<Queries.GetReviewsByProjectQuery>
{
    public GetReviewsByProjectQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}

using FluentValidation;

namespace DevCollab.Application.Features.Projects.Validators;

public class GetAllProjectsQueryValidator : AbstractValidator<Queries.GetAllProjectsQuery>
{
    public GetAllProjectsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}

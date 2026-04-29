using FluentValidation;

namespace DevCollab.Application.Features.Projects.Validators;

public class CreateProjectCommandValidator : AbstractValidator<Commands.CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CreateProjectDto.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.CreateProjectDto.Description).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.CreateProjectDto.RepositoryUrl)
            .Must(url => string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("RepositoryUrl must be a valid absolute URL.");
    }
}

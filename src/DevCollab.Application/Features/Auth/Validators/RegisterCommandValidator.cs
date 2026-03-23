using FluentValidation;

namespace DevCollab.Application.Features.Auth.Validators;

public class RegisterCommandValidator : AbstractValidator<Commands.RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.RegisterDto.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.RegisterDto.UserName).NotEmpty().MinimumLength(3);
        RuleFor(x => x.RegisterDto.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.RegisterDto.Role).InclusiveBetween(0, 1).WithMessage("Role must be 0 (Mentee) or 1 (Mentor).");
    }
}

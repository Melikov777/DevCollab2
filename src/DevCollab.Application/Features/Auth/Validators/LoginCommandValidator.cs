using FluentValidation;

namespace DevCollab.Application.Features.Auth.Validators;

public class LoginCommandValidator : AbstractValidator<Commands.LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.LoginDto.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.LoginDto.Password).NotEmpty();
    }
}

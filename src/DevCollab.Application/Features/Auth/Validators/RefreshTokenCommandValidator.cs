using FluentValidation;

namespace DevCollab.Application.Features.Auth.Validators;

public class RefreshTokenCommandValidator : AbstractValidator<Commands.RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshTokenDto.RefreshToken).NotEmpty();
    }
}

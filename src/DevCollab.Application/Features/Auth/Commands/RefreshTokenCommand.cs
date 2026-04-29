using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Auth.Commands;

public class RefreshTokenCommand : IRequest<AuthResponseDto>
{
    public RefreshTokenDto RefreshTokenDto { get; set; } = null!;
}

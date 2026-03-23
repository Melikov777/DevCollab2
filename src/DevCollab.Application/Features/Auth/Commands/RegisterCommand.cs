using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public RegisterDto RegisterDto { get; set; } = null!;
}

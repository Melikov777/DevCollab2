using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public LoginDto LoginDto { get; set; } = null!;
}

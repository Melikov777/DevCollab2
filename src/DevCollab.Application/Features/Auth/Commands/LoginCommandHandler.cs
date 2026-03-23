using DevCollab.Application.DTOs;
using DevCollab.Application.Interfaces;
using DevCollab.Domain.Exceptions;
using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IMapper _mapper;

    public LoginCommandHandler(IApplicationDbContext context, IJwtGenerator jwtGenerator, IMapper mapper)
    {
        _context = context;
        _jwtGenerator = jwtGenerator;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.LoginDto.Email, cancellationToken);

        if (user == null || user.PasswordHash != request.LoginDto.Password)
        {
            throw new DomainException("Invalid email or password.");
        }

        var token = _jwtGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            User = _mapper.Map<UserDto>(user)
        };
    }
}

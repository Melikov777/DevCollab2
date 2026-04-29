using AutoMapper;
using DevCollab.Application.DTOs;
using DevCollab.Application.Interfaces;
using DevCollab.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Application.Features.Auth.Commands;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IMapper _mapper;

    public RefreshTokenCommandHandler(IApplicationDbContext context, IJwtGenerator jwtGenerator, IMapper mapper)
    {
        _context = context;
        _jwtGenerator = jwtGenerator;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshTokenDto.RefreshToken, cancellationToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new DomainException("Invalid or expired refresh token.");
        }

        user.RefreshToken = Guid.NewGuid().ToString();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = user.RefreshToken,
            User = _mapper.Map<UserDto>(user)
        };
    }
}

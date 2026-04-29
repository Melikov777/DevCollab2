using DevCollab.Application.DTOs;
using DevCollab.Application.Interfaces;
using DevCollab.Domain.Entities;
using DevCollab.Domain.Enums;
using DevCollab.Domain.Exceptions;
using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IApplicationDbContext context, IJwtGenerator jwtGenerator, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _context = context;
        _jwtGenerator = jwtGenerator;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.RegisterDto.Email, cancellationToken))
            throw new DomainException("Email is already registered.");

        if (await _context.Users.AnyAsync(u => u.UserName == request.RegisterDto.UserName, cancellationToken))
            throw new DomainException("Username is already taken.");

        var user = new User
        {
            Email = request.RegisterDto.Email,
            UserName = request.RegisterDto.UserName,
            PasswordHash = _passwordHasher.HashPassword(request.RegisterDto.Password), 
            Role = (UserRole)request.RegisterDto.Role,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
        };

        if (user.Role == UserRole.Mentor)
        {
            user.MentorProfile = new MentorProfile
            {
                Bio = "New Mentor",
                Skills = "N/A"
            };
        }

        _context.Users.Add(user);
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

using AutoMapper;
using DevCollab.Application.DTOs;
using DevCollab.Application.Interfaces;
using DevCollab.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Application.Features.Projects.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = _mapper.Map<Project>(request.CreateProjectDto);
        project.UserId = request.UserId;

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);

        // Include user for mapping
        var createdProject = await _context.Projects
            .Include(p => p.User)
            .FirstAsync(p => p.Id == project.Id, cancellationToken);

        return _mapper.Map<ProjectDto>(createdProject);
    }
}

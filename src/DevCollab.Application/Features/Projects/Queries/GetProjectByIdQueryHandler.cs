using AutoMapper;
using DevCollab.Application.DTOs;
using DevCollab.Application.Interfaces;
using DevCollab.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Application.Features.Projects.Queries;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project == null)
            throw new NotFoundException(nameof(Domain.Entities.Project), request.ProjectId);

        return _mapper.Map<ProjectDto>(project);
    }
}

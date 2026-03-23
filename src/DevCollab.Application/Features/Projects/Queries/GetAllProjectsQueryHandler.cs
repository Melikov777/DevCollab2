using AutoMapper;
using DevCollab.Application.DTOs;
using DevCollab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Application.Features.Projects.Queries;

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProjectsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _context.Projects
            .Include(p => p.User)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ProjectDto>>(projects);
    }
}

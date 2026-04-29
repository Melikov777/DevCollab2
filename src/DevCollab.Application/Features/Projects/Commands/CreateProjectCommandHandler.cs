using AutoMapper;
using DevCollab.Application.DTOs;
using DevCollab.Application.Interfaces;
using DevCollab.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Application.Features.Projects.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IRepository<Project> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context; // Needed just for Include, but let's refactor.

    public CreateProjectCommandHandler(IRepository<Project> repository, IUnitOfWork unitOfWork, IMapper mapper, IApplicationDbContext context)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _context = context;
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = _mapper.Map<Project>(request.CreateProjectDto);
        project.UserId = request.UserId;

        await _repository.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Include user for mapping - Ideally we would use ProjectTo here, or return the ID and let the client fetch, but keeping existing logic for now.
        var createdProject = await _context.Projects
            .Include(p => p.User)
            .FirstAsync(p => p.Id == project.Id, cancellationToken);

        return _mapper.Map<ProjectDto>(createdProject);
    }
}

using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Projects.Commands;

public class CreateProjectCommand : IRequest<ProjectDto>
{
    public CreateProjectDto CreateProjectDto { get; set; } = null!;
    public Guid UserId { get; set; }
}

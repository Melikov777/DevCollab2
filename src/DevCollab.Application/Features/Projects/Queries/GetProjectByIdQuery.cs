using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Projects.Queries;

public class GetProjectByIdQuery : IRequest<ProjectDto>
{
    public Guid ProjectId { get; set; }
}

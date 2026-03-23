using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Projects.Commands;

public class DeleteProjectCommand : IRequest<bool>
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
}

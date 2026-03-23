using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Projects.Queries;

public class GetAllProjectsQuery : IRequest<List<ProjectDto>>
{
}

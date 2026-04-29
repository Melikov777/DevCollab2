using DevCollab.Application.DTOs;
using MediatR;

namespace DevCollab.Application.Features.Projects.Queries;

public class GetAllProjectsQuery : IRequest<PagedResult<ProjectDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

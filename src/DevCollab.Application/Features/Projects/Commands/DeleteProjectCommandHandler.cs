using DevCollab.Application.Interfaces;
using DevCollab.Domain.Exceptions;
using MediatR;

namespace DevCollab.Application.Features.Projects.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync(new object[] { request.ProjectId }, cancellationToken);

        if (project == null)
            throw new NotFoundException(nameof(Domain.Entities.Project), request.ProjectId);

        if (project.UserId != request.UserId)
            throw new DomainException("You do not have permission to delete this project.");

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

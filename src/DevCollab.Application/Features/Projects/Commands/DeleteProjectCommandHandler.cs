using DevCollab.Application.Interfaces;
using DevCollab.Domain.Exceptions;
using MediatR;

namespace DevCollab.Application.Features.Projects.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IRepository<Domain.Entities.Project> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectCommandHandler(IRepository<Domain.Entities.Project> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            throw new NotFoundException(nameof(Domain.Entities.Project), request.ProjectId);

        if (project.UserId != request.UserId)
            throw new DomainException("You do not have permission to delete this project.");

        project.IsDeleted = true;
        _repository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

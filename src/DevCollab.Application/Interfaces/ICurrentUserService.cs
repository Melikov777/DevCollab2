namespace DevCollab.Application.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}

using DevCollab.Domain.Entities;

namespace DevCollab.Application.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(User user);
}

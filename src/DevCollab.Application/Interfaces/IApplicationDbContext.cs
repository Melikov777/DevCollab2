using Microsoft.EntityFrameworkCore;
using DevCollab.Domain.Entities;

namespace DevCollab.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Project> Projects { get; }
    DbSet<Review> Reviews { get; }
    DbSet<Comment> Comments { get; }
    DbSet<MentorProfile> MentorProfiles { get; }
    DbSet<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

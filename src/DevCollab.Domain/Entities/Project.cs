namespace DevCollab.Domain.Entities;

public class Project : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? RepositoryUrl { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

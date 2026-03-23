namespace DevCollab.Domain.Entities;

public class Review : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public Guid ReviewerId { get; set; }
    public User Reviewer { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}

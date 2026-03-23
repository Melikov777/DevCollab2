namespace DevCollab.Domain.Entities;

public class Comment : BaseEntity
{
    public string Body { get; set; } = string.Empty;

    public Guid ReviewId { get; set; }
    public Review Review { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

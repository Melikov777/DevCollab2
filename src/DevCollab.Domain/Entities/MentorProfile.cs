namespace DevCollab.Domain.Entities;

public class MentorProfile : BaseEntity
{
    public string Bio { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

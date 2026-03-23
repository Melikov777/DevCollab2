namespace DevCollab.Application.DTOs;

public class ReviewDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public Guid ProjectId { get; set; }
    public Guid ReviewerId { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateReviewDto
{
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public Guid ProjectId { get; set; }
}

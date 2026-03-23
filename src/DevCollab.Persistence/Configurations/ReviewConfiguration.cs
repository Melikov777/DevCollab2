using DevCollab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevCollab.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Content)
            .IsRequired();
            
        builder.Property(r => r.Rating)
            .IsRequired();

        // 1-to-Many with Project
        builder.HasOne(r => r.Project)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1-to-Many with Reviewer (User)
        builder.HasOne(r => r.Reviewer)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

using DevCollab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevCollab.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Body)
            .IsRequired()
            .HasMaxLength(1000);

        // 1-to-Many with Review
        builder.HasOne(c => c.Review)
            .WithMany(r => r.Comments)
            .HasForeignKey(c => c.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1-to-Many with User
        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

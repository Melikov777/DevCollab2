using DevCollab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevCollab.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(p => p.RepositoryUrl)
            .HasMaxLength(500);

        // 1-to-Many relationship with User
        builder.HasOne(p => p.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete 
    }
}

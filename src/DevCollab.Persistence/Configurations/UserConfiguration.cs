using DevCollab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevCollab.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);
            
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(u => u.UserName)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        // 1-to-1 relationship with MentorProfile
        builder.HasOne(u => u.MentorProfile)
            .WithOne(mp => mp.User)
            .HasForeignKey<MentorProfile>(mp => mp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

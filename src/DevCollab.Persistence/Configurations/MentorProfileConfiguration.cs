using DevCollab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevCollab.Persistence.Configurations;

public class MentorProfileConfiguration : IEntityTypeConfiguration<MentorProfile>
{
    public void Configure(EntityTypeBuilder<MentorProfile> builder)
    {
        builder.HasKey(mp => mp.Id);
        
        builder.Property(mp => mp.Bio)
            .HasMaxLength(1000);
            
        builder.Property(mp => mp.Skills)
            .HasMaxLength(500);
    }
}

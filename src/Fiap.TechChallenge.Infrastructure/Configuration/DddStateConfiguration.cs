using Fiap.TechChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.TechChallenge.Infrastructure.Configuration;

public class DddStateConfiguration: IEntityTypeConfiguration<DddState>
{
    public void Configure(EntityTypeBuilder<DddState> builder)
    {
        builder.HasKey(k => k.DddNumber);
        
        builder.Property(k => k.StateName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(p => p.StateAbbreviation)
            .IsRequired()
            .HasMaxLength(2);
        
        builder.Property(p => p.DddNumber)
            .HasMaxLength(3)
            .IsRequired();
    }
}
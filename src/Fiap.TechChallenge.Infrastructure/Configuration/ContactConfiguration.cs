using Fiap.TechChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.TechChallenge.Infrastructure.Configuration;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(9);
        builder
            .Property(p => p.DddNumber)
            .IsRequired();
        builder
            .HasOne<DddState>()
            .WithMany()
            .HasForeignKey(f => f.DddNumber);
    }
}
using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Fiap.TechChallenge.Infrastructure.Context;

public class ContactDbContext(DbContextOptions<ContactDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<DddState> DddStates { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactConfiguration).Assembly);
    }
}
using AutoFixture;
using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.TechChallenge.IntegrationTests.Helpers;

public class ContactHelper
{
    public async Task<Contact> CreateContact(CustomWebApplicationFactory factory, int dddNumber = 11, string phoneNumber = "999999999", string email = "integration@test.com")
    {
        ArgumentNullException.ThrowIfNull(factory);

        var fixture = new Fixture();
        var contact = fixture
            .Build<Contact>()
            .With(x=>x.DddNumber, dddNumber)
            .With(x=>x.PhoneNumber, phoneNumber)
            .With(x=>x.Email, email)
            .Create();
        
        var cancellationTokenSource = new CancellationTokenSource();
        await using var scope = factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
        await dbContext.Contacts.AddAsync(contact, cancellationTokenSource.Token);
        await dbContext.SaveChangesAsync(cancellationTokenSource.Token);
        
        return contact;
    }
}
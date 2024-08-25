using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Fiap.TechChallenge.Domain.Request;
using Fiap.TechChallenge.Infrastructure.Context;
using Fiap.TechChallenge.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.TechChallenge.IntegrationTests.Contacts;

[Collection("contactsTests")]
public class UpdateContactsTests(CustomWebApplicationFactory factory)
{
    private readonly Fixture _fixture = new ();
    private readonly ContactHelper _contactHelper = new();
    
    [Fact]
    public async Task ShouldUpdateContactAndReturnStatusCode200()
    {
        // Arrange
        var contact = await _contactHelper.CreateContact(factory);

        var contactPutRequest = _fixture
            .Build<ContactPutRequest>()
            .With(x=>x.Id, contact.Id)
            .With(x=>x.Ddd, 12)
            .With(x=>x.PhoneNumber, "999999998")
            .With(x=>x.Email, "integration2@test.com")
            .Create();
        
        var client = factory.CreateClient();
        var cancellationTokenSource = new CancellationTokenSource();
        // Act
        var response = await client.PutAsJsonAsync($"api/contact/{contactPutRequest.Id}", contactPutRequest, cancellationTokenSource.Token);
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var scope = factory.Services.CreateAsyncScope();
        var dbContext2 = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
        var savedContact = await dbContext2.Contacts.FindAsync(contactPutRequest.Id, cancellationTokenSource.Token);
        savedContact.Should().NotBeNull();
        savedContact!.PhoneNumber.Should().Be(contactPutRequest.PhoneNumber);
        savedContact.Email.Should().Be(contactPutRequest.Email);
        savedContact.DddNumber.Should().Be(contactPutRequest.Ddd);
        savedContact.Name.Should().Be(contactPutRequest.Name);
    }
}
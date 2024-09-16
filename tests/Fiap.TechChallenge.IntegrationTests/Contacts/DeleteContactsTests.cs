using System.Net;
using Fiap.TechChallenge.Infrastructure.Context;
using Fiap.TechChallenge.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.TechChallenge.IntegrationTests.Contacts;

[Collection("contactsTests")]
public class DeleteContactsTests(CustomWebApplicationFactory factory)
{
    private readonly ContactHelper _contactHelper = new();
    
    [Fact]
    public async Task ShouldDeleteContactAndReturnStatusCode200()
    {
        // Arrange
        var contact = await _contactHelper.CreateContact(factory);
        
        var client = factory.CreateClient();
        
        var cancellationTokenSource = new CancellationTokenSource();
        // Act
        var response = await client.DeleteAsync($"api/contact/{contact.Id}", cancellationTokenSource.Token);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }
}
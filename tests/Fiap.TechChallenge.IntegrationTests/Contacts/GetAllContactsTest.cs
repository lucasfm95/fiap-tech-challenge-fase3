using System.Net;
using System.Net.Http.Json;
using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.IntegrationTests.Helpers;
using FluentAssertions;

namespace Fiap.TechChallenge.IntegrationTests.Contacts;

public class GetAllContactsTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly ContactHelper _contactHelper = new();
    
    [Fact]
    public async Task GetAllContactsShouldReturnAllContactsCorrectly()
    {
        // Arrange
        var contact1 = await _contactHelper.CreateContact(factory);
        var contact2 = await _contactHelper.CreateContact(factory);
        var contact3 = await _contactHelper.CreateContact(factory);
        
        var client = factory.CreateClient();
        
        var cancellationTokenSource = new CancellationTokenSource();
        // Act
        var response = await client.GetAsync($"api/contact", cancellationTokenSource.Token);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var contactResponse = await response.Content.ReadFromJsonAsync<List<Contact>>(cancellationTokenSource.Token);
        contactResponse.Should().NotBeNull().And.NotBeEmpty();
        contactResponse.Should().BeEquivalentTo(new List<Contact>(){contact1, contact2, contact3});
    }
}
using System.Net;
using System.Net.Http.Json;
using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.IntegrationTests.Helpers;
using FluentAssertions;

namespace Fiap.TechChallenge.IntegrationTests.Contacts;

[Collection("contactsTests")]
public class GetContactsTests(CustomWebApplicationFactory factory)
{
    private readonly ContactHelper _contactHelper = new();
    
    [Fact]
    public async Task GetByIdShouldReturnSavedContact()
    {
        // Arrange
        var contact = await _contactHelper.CreateContact(factory);
        
        var client = factory.CreateClient();
        
        var cancellationTokenSource = new CancellationTokenSource();
        // Act
        var response = await client.GetAsync($"api/contact/{contact.Id}", cancellationTokenSource.Token);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var contactResponse = await response.Content.ReadFromJsonAsync<Contact>(cancellationTokenSource.Token);
        contactResponse.Should().NotBeNull();
        contactResponse.Should().BeEquivalentTo(contact);
    }
    
    [Fact]
    public async Task GetDddShouldReturnSavedContactCorrectly()
    {
        // Arrange
        _ = await _contactHelper.CreateContact(factory);
        var contact2 = await _contactHelper.CreateContact(factory, dddNumber:12);
        var contact3 = await _contactHelper.CreateContact(factory, dddNumber:12);
        
        var client = factory.CreateClient();
        
        var cancellationTokenSource = new CancellationTokenSource();
        // Act
        var response = await client.GetAsync($"api/contact/ddd/{contact2.DddNumber}", cancellationTokenSource.Token);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var contactResponse = await response.Content.ReadFromJsonAsync<List<Contact>>(cancellationTokenSource.Token);
        contactResponse.Should().NotBeNull().And.NotBeEmpty();
        contactResponse.Should().BeEquivalentTo(new List<Contact>(){contact2,contact3});
    }
}
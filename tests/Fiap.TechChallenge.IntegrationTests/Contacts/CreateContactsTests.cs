using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Fiap.TechChallenge.Application.Repositories;
using Fiap.TechChallenge.Domain.Request;
using Fiap.TechChallenge.Domain.Response;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.TechChallenge.IntegrationTests.Contacts;

[Collection("contactsTests")]
public class CreateContactsTests(CustomWebApplicationFactory factory)
{
    private readonly Fixture _fixture = new ();
    [Fact]
    public async Task ShouldCreateContactAndReturnStatusCode201()
    {
        // Arrange
        var contactPostRequest = _fixture
            .Build<ContactPostRequest>()
            .With(x=>x.Ddd, 11)
            .With(x=>x.PhoneNumber, "999999999")
            .With(x=>x.Email, "integration@test.com")
            .Create();
        
        var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/contact", contactPostRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }
}
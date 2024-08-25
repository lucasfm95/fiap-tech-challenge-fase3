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
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var id = long.Parse(response.Headers.Location!.Segments.Last());
        var postResponse = await response.Content.ReadFromJsonAsync<ContactPostResponse>();
        await using var scope = factory.Services.CreateAsyncScope();
        var contactRepository = scope.ServiceProvider.GetRequiredService<IContactRepository>();
        var contactInDb = await contactRepository.FindByIdAsync(id, CancellationToken.None);
        contactInDb!.PhoneNumber.Should().Be(postResponse!.PhoneNumber).And.Be(contactPostRequest.PhoneNumber);
        contactInDb.Email.Should().Be(postResponse.Email).And.Be(contactPostRequest.Email);
        contactInDb.DddNumber.Should().Be(postResponse.Ddd).And.Be(contactPostRequest.Ddd);
        contactInDb.Name.Should().Be(postResponse.Name).And.Be(contactPostRequest.Name);
    }
}
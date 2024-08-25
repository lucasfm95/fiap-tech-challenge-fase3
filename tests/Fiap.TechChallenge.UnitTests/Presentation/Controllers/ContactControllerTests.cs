using System.Net;
using AutoFixture;
using Fiap.TechChallenge.Api.Controllers;
using Fiap.TechChallenge.Application.Services.Interfaces;
using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.Domain.Request;
using Fiap.TechChallenge.Domain.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.TechChallenge.UnitTests.Presentation.Controllers;

public class ContactControllerTests
{
    private readonly Fixture _fixture = new ();
    
    [Fact]
    private async Task GetAllShouldBeSuccess()
    {
        var mockContactService = new Mock<IContactService>();
        var cancellationToken = new CancellationToken();
        
        var contacts = new List<Contact>
        {
            new("John Doe", "john@email.com", "123456789", 11),
            new("Jane Doe", "jane@email.com", "987654321", 21)
        };
        
        mockContactService
            .Setup(contactService => contactService.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(contacts);
        
        var controller = new ContactController(mockContactService.Object, It.IsAny<ILogger<ContactController>>());

        var result = await controller.GetAll(cancellationToken) as OkObjectResult;
        
        result.Should().BeOfType<OkObjectResult>().And.NotBeNull();
        result?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result?.Value.Should().BeEquivalentTo(contacts);
        mockContactService.Verify(contactService => contactService.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    private async Task GetAllShouldBeSuccessWhenListOfContactsIsEmpty()
    {
        var mockContactService = new Mock<IContactService>();
        var cancellationToken = new CancellationToken();
        
        var contacts = new List<Contact>();
        
        mockContactService
            .Setup(contactService => contactService.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(contacts);
        
        var controller = new ContactController(mockContactService.Object, It.IsAny<ILogger<ContactController>>());

        var result = await controller.GetAll(cancellationToken) as OkObjectResult;
        
        result.Should().BeOfType<OkObjectResult>().And.NotBeNull();
        result?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result?.Value.Should().BeEquivalentTo(contacts);
        mockContactService.Verify(contactService => contactService.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    private async Task GetByIdShouldBeSuccess()
    {
        var mockContactService = new Mock<IContactService>();
        var cancellationToken = new CancellationToken();

        var contact = new Contact("John Doe", "john@email.com", "123456789", 11);

        mockContactService
            .Setup(contactService => contactService.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contact);

        var controller = new ContactController(mockContactService.Object, It.IsAny<ILogger<ContactController>>());

        var result = await controller.GetById(1, cancellationToken) as OkObjectResult;

        result.Should().BeOfType<OkObjectResult>().And.NotBeNull();
        result?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result?.Value.Should().Be(contact);
        mockContactService.Verify(contactService => contactService.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    private async Task GetByIdShouldBeNoContent()
    {
        var mockContactService = new Mock<IContactService>();
        var cancellationToken = new CancellationToken();

        mockContactService
            .Setup(contactService => contactService.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contact)null!);

        var controller = new ContactController(mockContactService.Object, It.IsAny<ILogger<ContactController>>());

        var result = await controller.GetById(1, cancellationToken) as NoContentResult;

        result.Should().BeOfType<NoContentResult>().And.NotBeNull();
        result?.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        mockContactService.Verify(contactService => contactService.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    private async Task GetByDddShouldBeSuccess()
    {
        var mockContactService = new Mock<IContactService>();
        var cancellationToken = new CancellationToken();

        var contacts = new List<Contact>
        {
            new("John Doe", "john@email.com", "123456789", 11),
            new("Tom Doe", "tom@email.com", "123451234", 11),
            new("Jane Doe", "jane@email.com", "987654321", 21)
        };

        var contactsWithDdd11 = contacts.Where(c => c.DddNumber == 11).ToList();

        mockContactService
            .Setup(contactService => contactService.GetAllByDddAsync(It.IsAny<short>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contactsWithDdd11);

        var controller = new ContactController(mockContactService.Object, It.IsAny<ILogger<ContactController>>());

        var result = await controller.GetByDdd(11, cancellationToken) as OkObjectResult;

        result.Should().BeOfType<OkObjectResult>().And.NotBeNull();
        result?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result?.Value.Should().Be(contactsWithDdd11);
        result?.Value.Should().NotBe(contacts);
        mockContactService.Verify(contactService => contactService.GetAllByDddAsync(It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    private async Task GetByDddShouldBeNoContent()
    {
        var mockContactService = new Mock<IContactService>();
        var cancellationToken = new CancellationToken();
        
        mockContactService
            .Setup(contactService => contactService.GetAllByDddAsync(It.IsAny<short>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Contact>());

        var controller = new ContactController(mockContactService.Object, It.IsAny<ILogger<ContactController>>());

        var result = await controller.GetByDdd(99, cancellationToken) as NoContentResult;

        result.Should().BeOfType<NoContentResult>().And.NotBeNull();
        result?.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        mockContactService.Verify(contactService => contactService.GetAllByDddAsync(It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    private async Task CreateShouldSuccess()
    {
        var mockContactService = new Mock<IContactService>();
        var cancellationToken = new CancellationToken();
        var contactPostRequest = _fixture.Create<ContactPostRequest>();
        var contact = _fixture.Build<Contact>()
            .With(c => c.Name, contactPostRequest.Name)
            .With(c => c.DddNumber, contactPostRequest.Ddd)
            .With(c => c.PhoneNumber, contactPostRequest.PhoneNumber)
            .With(c=>c.Email, contactPostRequest.Email)
            .With(c=>c.Id, 1)
            .Create();

        var contactPostResponse = new ContactPostResponse(contact.DddNumber, contact.Email,  contact.PhoneNumber, contact.Name);
        
        mockContactService
            .Setup(contactService => contactService.CreateAsync(contactPostRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contact);

        var controller = new ContactController(mockContactService.Object, It.IsAny<ILogger<ContactController>>());

        var result = await controller.Create(contactPostRequest, cancellationToken);
        
        mockContactService.Verify(contactService => contactService.CreateAsync(contactPostRequest, It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<CreatedAtActionResult>().And.NotBeNull();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
        createdResult.Value.Should().BeEquivalentTo(contactPostResponse);
        createdResult.ActionName.Should().Be(nameof(controller.GetById));
    }
    
    [Fact]
    private async Task UpdateShouldBeSuccess()
    {
        var mockContactService = new Mock<IContactService>();
        var cancellationToken = new CancellationToken();
        var contactPutRequest = _fixture
            .Build<ContactPutRequest>()
            .With(c=>c.Id, 1)
            .Create();
        
        var contact = _fixture.Build<Contact>()
            .With(c => c.Name, contactPutRequest.Name)
            .With(c => c.DddNumber, contactPutRequest.Ddd)
            .With(c => c.PhoneNumber, contactPutRequest.PhoneNumber)
            .With(c=>c.Email, contactPutRequest.Email)
            .With(c=>c.Id, contactPutRequest.Id)
            .Create();

        var contactPutResponse = new ContactPutResponse(contact.DddNumber, contact.Email,  contact.PhoneNumber, contact.Name);
        
        mockContactService
            .Setup(contactService => contactService.UpdateAsync(contactPutRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contact);

        var controller = new ContactController(mockContactService.Object, It.IsAny<ILogger<ContactController>>());

        var result = await controller.Update(contactPutRequest.Id, contactPutRequest, cancellationToken);
        
        mockContactService.Verify(contactService => contactService.UpdateAsync(contactPutRequest, It.IsAny<CancellationToken>()), Times.Once);
        var okObjectResult = result as OkObjectResult;
        okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okObjectResult.Value.Should().BeEquivalentTo(new DefaultResponse<ContactPutResponse>(){Data = contactPutResponse, Message = "Contact updated successfully."});
    }
    
    [Fact]
    private async Task DeleteShouldBeSuccess()
    {
        var mockContactService = new Mock<IContactService>();
        
        mockContactService.Setup(x =>
            x.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        var controller = new ContactController(mockContactService.Object, It.IsAny<ILogger<ContactController>>());

        var result = await controller.Delete(1, new CancellationToken()) as OkObjectResult;
        
        result.Should().BeOfType<OkObjectResult>().And.NotBeNull();
        result?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mockContactService.Verify(contactService => contactService.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    private async Task DeleteShouldReturnBadRequest()
    {
        var mockContactService = new Mock<IContactService>();
        
        mockContactService.Setup(x =>
                x.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .Throws<Exception>();
        
        var controller = new ContactController(mockContactService.Object, It.IsAny<ILogger<ContactController>>());

        await Assert.ThrowsAsync<Exception>(() => controller.Delete(1, new CancellationToken()));
        
        mockContactService.Verify(contactService => contactService.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
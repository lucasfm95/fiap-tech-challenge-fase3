using AutoFixture;
using Fiap.TechChallenge.Application.MessageBroker;
using Fiap.TechChallenge.Application.Repositories;
using Fiap.TechChallenge.Application.Services;
using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.Domain.Request;
using Fiap.TechChallenge.LibDomain.Events;
using FluentAssertions;
using Moq;

namespace Fiap.TechChallenge.UnitTests.Application.Services;

public class ContactServiceTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task ShouldCreateWithSuccessAsync()
    {
        // Arrange
        var contact = _fixture
            .Build<ContactPostRequest>()
            .With(c => c.Email, "Email@teste.com")
            .With(c => c.PhoneNumber, "123456789")
            .With(c => c.Name, "Teste")
            .With(c => c.Ddd, 11)
            .Create();

        var contactRepository = new Mock<IContactRepository>();

        var publisherServiceMock = new Mock<IPublisherService>();
        publisherServiceMock
            .Setup(publisher => publisher.SendToProcessInsertAsync(It.IsAny<ContactInsertEvent>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var contactService = new ContactService(contactRepository.Object, publisherServiceMock.Object);

        // Act
        var result = await contactService.CreateAsync(contact, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        publisherServiceMock.Verify(
            publisher => publisher.SendToProcessInsertAsync(It.IsAny<ContactInsertEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldUpdateWithSuccessAsync()
    {
        // Arrange
        var contactPutRequest = _fixture
            .Build<ContactPutRequest>()
            .With(c => c.Email, "Email@teste.com")
            .With(c => c.PhoneNumber, "123456789")
            .With(c => c.Name, "Teste")
            .With(c => c.Ddd, 11)
            .With(c => c.Id, 1)
            .Create();

        var contactRepository = new Mock<IContactRepository>();

        var publisherServiceMock = new Mock<IPublisherService>();
        publisherServiceMock
            .Setup(publisher => publisher.SendToProcessUpdateAsync(It.IsAny<ContactUpdateEvent>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var contact = new Contact("teste 2", "teste@email.com", "123456788", 12) { Id = contactPutRequest.Id };

        contactRepository.Setup(x => x.FindByIdAsync(contactPutRequest.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contact);

        var contactService = new ContactService(contactRepository.Object, publisherServiceMock.Object);

        // Act
        var result = await contactService.UpdateAsync(contactPutRequest, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        publisherServiceMock.Verify(
            publisher => publisher.SendToProcessUpdateAsync(It.IsAny<ContactUpdateEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldDeleteWithSuccess()
    {
        var contact = _fixture
            .Build<Contact>()
            .With(c => c.Email, "Email@teste.com")
            .With(c => c.PhoneNumber, "123456789")
            .With(c => c.Name, "Teste")
            .With(c => c.DddNumber, 11)
            .Create();

        //Arrange
        var contactRepository = new Mock<IContactRepository>();
        contactRepository.Setup(c => c.FindByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contact);

        var publisherServiceMock = new Mock<IPublisherService>();
        publisherServiceMock
            .Setup(publisher => publisher.SendToProcessDeleteAsync(It.IsAny<ContactDeleteEvent>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var contactService = new ContactService(contactRepository.Object, publisherServiceMock.Object);

        //Act
        var result = await contactService.DeleteAsync(1, CancellationToken.None);

        //Assert
        result.Should().BeTrue();
        publisherServiceMock.Verify(c => c.SendToProcessDeleteAsync(It.IsAny<ContactDeleteEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    private async Task ShouldGetAllWithSuccess()
    {
        var contactRepository = new Mock<IContactRepository>();

        contactRepository.Setup(c => c.FindAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Contact>());

        var contactService = new ContactService(contactRepository.Object, null);

        var result = await contactService.GetAllAsync(CancellationToken.None);

        result.Should().NotBeNull();
        contactRepository.Verify(c =>
            c.FindAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    private async Task ShouldGetAllByDddWithSuccess()
    {
        var contactRepository = new Mock<IContactRepository>();

        contactRepository.Setup(c => c.FindAllByDddAsync(It.IsAny<short>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Contact>());

        var contactService = new ContactService(contactRepository.Object, null);

        var result = await contactService.GetAllByDddAsync(11, CancellationToken.None);

        result.Should().NotBeNull();
        contactRepository.Verify(c =>
            c.FindAllByDddAsync(It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    private async Task ShouldGetByIdWithSuccess()
    {
        var contactRepository = new Mock<IContactRepository>();
        var contact = new Contact("John Doe", "john@email.com", "123456789", 11);

        contactRepository.Setup(c => c.FindByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contact);

        var contactService = new ContactService(contactRepository.Object, null);

        var result = await contactService.GetByIdAsync(1, CancellationToken.None);

        result.Should().BeOfType<Contact>().And.NotBeNull();
        result.Should().Be(contact);
        contactRepository.Verify(c =>
            c.FindByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
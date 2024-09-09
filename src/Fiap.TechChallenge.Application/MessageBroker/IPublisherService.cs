using Fiap.TechChallenge.Domain.Entities;

namespace Fiap.TechChallenge.Application.MessageBroker;

public interface IPublisherService
{
    Task Publish(Contact contact);
}
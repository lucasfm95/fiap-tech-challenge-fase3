using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.LibDomain.Events;

namespace Fiap.TechChallenge.Application.MessageBroker;

public interface IPublisherService
{
    Task<bool> SendToProcessInsertAsync(ContactInsertEvent contractInsertEvent, CancellationToken cancellationToken);
    Task<bool> SendToProcessUpdateAsync(ContactUpdateEvent contractUpdateEvent, CancellationToken cancellationToken); 
    Task<bool> SendToProcessDeleteAsync(ContactDeleteEvent contractDeleteEvent, CancellationToken cancellationToken);
}
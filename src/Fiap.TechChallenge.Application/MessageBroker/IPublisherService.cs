using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.LibDomain.Events;

namespace Fiap.TechChallenge.Application.MessageBroker;

public interface IPublisherService
{
    Task SendToProcessInsertAsync(ContactInsertEvent contractInsertEvent, CancellationToken cancellationToken);
    Task SendToProcessUpdateAsync(ContactUpdateEvent contractUpdateEvent, CancellationToken cancellationToken); 
    Task SendToProcessDeleteAsync(ContactDeleteEvent contractDeleteEvent, CancellationToken cancellationToken);
}
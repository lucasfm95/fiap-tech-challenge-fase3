using Fiap.TechChallenge.Application.MessageBroker;
using Fiap.TechChallenge.LibDomain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Infrastructure.MessageBroker;

public class PublisherService(IBus bus, ILogger<PublisherService> logger) : IPublisherService
{
    public async Task<bool> SendToProcessInsertAsync(ContactInsertEvent contractInsertEvent, CancellationToken cancellationToken)
    {
        var queueName = Environment.GetEnvironmentVariable("MASS_TRANSIT_INSERT_QUEUE_NAME") ?? string.Empty;
        return await PublishAsync(contractInsertEvent, queueName, cancellationToken);
    }
    public async Task<bool> SendToProcessUpdateAsync(ContactUpdateEvent contractUpdateEvent, CancellationToken cancellationToken)
    {
        var queueName = Environment.GetEnvironmentVariable("MASS_TRANSIT_UPDATE_QUEUE_NAME") ?? string.Empty;
        return await PublishAsync(contractUpdateEvent, queueName, cancellationToken);
    }
    
    public async Task<bool> SendToProcessDeleteAsync(ContactDeleteEvent contractDeleteEvent, CancellationToken cancellationToken)
    {
        var queueName = Environment.GetEnvironmentVariable("MASS_TRANSIT_DELETE_QUEUE_NAME") ?? string.Empty;
        return await PublishAsync(contractDeleteEvent, queueName, cancellationToken);
    }
    
    private async Task<bool> PublishAsync<T>(T message, string queueName, CancellationToken cancellationToken)
    {
        try
        {
            if (message is not null)
            {
                var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{queueName}"));
        
                await endpoint.Send(message, cancellationToken);
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error publishing message to queue {QueueName}", queueName);
        } 
        
        return false;
    }
}
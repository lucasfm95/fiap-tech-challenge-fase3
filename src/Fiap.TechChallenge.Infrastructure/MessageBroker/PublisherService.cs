using Fiap.TechChallenge.Application.MessageBroker;
using Fiap.TechChallenge.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Infrastructure.MessageBroker;

public class PublisherService(IBus bus, ILogger<PublisherService> logger) : IPublisherService
{
    public async Task Publish(Contact contact, CancellationToken cancellationToken)
    {
        try
        {
            var queueName = Environment.GetEnvironmentVariable("MASS_TRANSIT_QUEUE_NAME") ?? string.Empty;
        
            var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{queueName}"));
        
            await endpoint.Send(contact, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}
using System.Text.Json;
using Fiap.TechChallenge.Application.MessageBroker;
using Fiap.TechChallenge.Domain.Entities;
using MassTransit;

namespace Fiap.TechChallenge.Infrastructure.MessageBroker;

public class PublisherService(IBus bus) : IPublisherService
{
    public async Task Publish(Contact contact)
    {
        try
        {
            var queueName = Environment.GetEnvironmentVariable("MASS_TRANSIT_QUEUE_NAME") ?? string.Empty;
        
            var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{queueName}"));
        
            await endpoint.Send(contact);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}
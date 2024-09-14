using Fiap.TechChallenge.Application.MessageBroker;
using Fiap.TechChallenge.Application.Repositories;
using Fiap.TechChallenge.Application.Services.Interfaces;
using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.Domain.Request;
using Fiap.TechChallenge.LibDomain.Events;
using FluentValidation;

namespace Fiap.TechChallenge.Application.Services;

public class ContactService(IContactRepository contactRepository, IPublisherService publisherService) : IContactService
{
    public async Task<List<Contact>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await contactRepository.FindAllAsync(cancellationToken);
    }
    
    public async Task<List<Contact>> GetAllByDddAsync(short dddNumber, CancellationToken cancellationToken)
    {
        return await contactRepository.FindAllByDddAsync(dddNumber, cancellationToken);
    }
    
    public async Task<Contact?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await contactRepository.FindByIdAsync(id, cancellationToken);
    }
    
    public async Task<bool> CreateAsync(ContactPostRequest request, CancellationToken cancellationToken)
    {
        var validator = new ContactPostRequestValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var contactInsertEvent = new ContactInsertEvent(request.Name,request.Email, request.PhoneNumber, request.Ddd);
        
        return await publisherService.SendToProcessInsertAsync(contactInsertEvent, cancellationToken);
    }
    
    public async Task<bool> UpdateAsync(ContactPutRequest request, CancellationToken cancellationToken)
    {
        var contact = await GetByIdAsync(request.Id, cancellationToken);

        if (contact is null)
        {
            throw new ValidationException($"Contact with id {request.Id} not found.");
        }

        var validator = new ContactPutRequestValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        contact.Name = request.Name;
        contact.Email = request.Email;
        contact.PhoneNumber = request.PhoneNumber;
        contact.DddNumber = request.Ddd;
        
        var contactUpdateEvent = new ContactUpdateEvent( request.Name, request.Email, request.PhoneNumber, request.Ddd) { Id = request.Id };
        
        return await publisherService.SendToProcessUpdateAsync(contactUpdateEvent, cancellationToken);
    }
    
    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var contact = await GetByIdAsync(id, cancellationToken);
        
        if (contact is null)
        {
            throw new ValidationException($"Contact with id {id} not found.");
        }
        
        var contactDeleteEvent = new ContactDeleteEvent() { Id = id };
        
        return await publisherService.SendToProcessDeleteAsync(contactDeleteEvent, cancellationToken);
    }
}
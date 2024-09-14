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
    
    public async Task<Contact> CreateAsync(ContactPostRequest request, CancellationToken cancellationToken)
    {
        var validator = new ContactPostRequestValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var contact = new ContactInsertEvent(request.Name,request.Email, request.PhoneNumber, request.Ddd);
        //var result = await contactRepository.CreateAsync(contact, cancellationToken);
        await publisherService.SendToProcessInsertAsync(contact, cancellationToken);
        return null;
    }
    
    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
       return await contactRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<Contact> UpdateAsync(ContactPutRequest request, CancellationToken cancellationToken)
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
        
        await contactRepository.UpdateAsync(contact, cancellationToken);
        
        return contact;
    }
}
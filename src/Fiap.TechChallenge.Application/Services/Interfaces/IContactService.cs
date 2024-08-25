using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.Domain.Request;

namespace Fiap.TechChallenge.Application.Services.Interfaces;

public interface IContactService
{
    Task<List<Contact>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<Contact>> GetAllByDddAsync(short dddNumber, CancellationToken cancellationToken);
    Task<Contact?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<Contact> CreateAsync(ContactPostRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);

    Task<Contact> UpdateAsync(ContactPutRequest request, CancellationToken cancellationToken);
}
using Fiap.TechChallenge.Domain.Entities;

namespace Fiap.TechChallenge.Application.Repositories;

/// <summary>
/// Interface repository to manage contact data from database.
/// </summary>
/// <param name="dbContext">Entity framework database context.</param>
public interface IContactRepository
{
    /// <summary>
    /// Method to find all contacts.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Return a list of contact object from database.</returns>
    public Task<List<Contact>> FindAllAsync(CancellationToken cancellationToken);
    /// <summary>
    /// Method to find all contacts by ddd.
    /// </summary>
    /// <param name="dddNumber"></param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Return a list of contact object from database.</returns>
    public Task<List<Contact>> FindAllByDddAsync(short dddNumber, CancellationToken cancellationToken);
    /// <summary>
    /// Method to find a contact by id.
    /// </summary>
    /// <param name="id">Unique contact identifier.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Return a unique contact object from database.</returns>
    public Task<Contact?> FindByIdAsync(long id, CancellationToken cancellationToken);
    public Task<Contact> CreateAsync(Contact contact, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    public Task<bool> UpdateAsync(Contact contact, CancellationToken cancellationToken);
}
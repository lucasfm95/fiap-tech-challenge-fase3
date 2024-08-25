using Fiap.TechChallenge.Application.Repositories;
using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.TechChallenge.Infrastructure.Repositories;

/// <summary>
/// Class repository to manage contact data from database.
/// </summary>
/// <param name="dbContext">Entity framework database context.</param>
public class ContactRepository(ContactDbContext dbContext) : IContactRepository
{
    /// <summary>
    /// Method to find all contacts.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Return a list of contact object from database.</returns>
    public async Task<List<Contact>> FindAllAsync(CancellationToken cancellationToken)
    {
        var contacts = await dbContext.Contacts.ToListAsync(cancellationToken);
        return contacts;
    }

    /// <summary>
    /// Method to find all contacts by ddd.
    /// </summary>
    /// <param name="dddNumber"></param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Return a list of contact object from database.</returns>
    public async Task<List<Contact>> FindAllByDddAsync(short dddNumber, CancellationToken cancellationToken)
    {
        var contacts = await dbContext.Contacts.Where(contact => contact.DddNumber == dddNumber).ToListAsync(cancellationToken);
        return contacts;
    }

    /// <summary>
    /// Method to find a contact by id.
    /// </summary>
    /// <param name="id">Unique contact identifier.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Return a unique contact object from database.</returns>
    public async Task<Contact?> FindByIdAsync(long id, CancellationToken cancellationToken)
    {
        var contact = await dbContext.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken);
        return contact;
    }

    public async Task<Contact> CreateAsync(Contact contact, CancellationToken cancellationToken)
    {
        await dbContext.Contacts.AddAsync(contact, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return contact;
    }

    /// <summary>
    ///  Method to remove a contact 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var contact = dbContext.Contacts.FirstOrDefault(c => c.Id == id);
        if (contact == null)
        {
            return false;
        }
        dbContext.Contacts.Remove(contact);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Method to update / replace a contact
    /// </summary>
    /// <param name="contact">Contact to update</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(Contact contact, CancellationToken cancellationToken)
    {
        dbContext.Contacts.Update(contact);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
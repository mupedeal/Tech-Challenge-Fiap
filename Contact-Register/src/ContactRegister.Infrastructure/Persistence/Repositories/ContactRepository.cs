using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactRegister.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly DbSet<Contact> _contacts;
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
        _contacts = context.Contacts;
    }
    
    public async Task AddContactAsync(Contact contact)
    {
		_ = await _contacts.AddAsync(contact);
        _ = await _context.SaveChangesAsync();
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _contacts.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync()
    {
        return await _contacts.ToListAsync();
    }

    public async Task UpdateContactAsync(Contact contact)
    {
		_contacts.Update(contact);
        _ = await _context.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(Contact contact)
    {
        _contacts.Remove(contact); 
        await _context.SaveChangesAsync();
    }

    public async Task<List<Contact>> GetContactsByDdd(int[] codes) =>
        await _contacts
            .Where(c => codes.Contains(c.Ddd))
            .ToListAsync();
}
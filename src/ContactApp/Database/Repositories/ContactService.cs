using ContactApp.DTO;
using ContactApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Database.Repositories;

public interface IContactService
{
    Task<int> SaveOrUpdateContact(ContactDTO dto);
    Task<ContactDTO> GetContactById(int id);
    Task<IEnumerable<ContactDTO>> GetAllContacts();
}

public class ContactService : IContactService
{
    private readonly DatabaseContext db;

    public ContactService(DatabaseContext databaseContext)
    {
        db = databaseContext;
    }

    public async Task<IEnumerable<ContactDTO>> GetAllContacts()
    {
        return db.Contacts.Select(model => new ContactDTO
        {
            Email = model.Email,
            Company = model.Company,
            FirstName = model.FirstName,
            Surname = model.Surname,
        });
    }

    private async Task<Contact> GetContactModelById(int id)
    {
        var model = await db.Contacts.Include(s=>s.PhoneNumbers).SingleOrDefaultAsync(s => s.Id == id);
        if (model == null)
        {
            return null;
        }

        return model;
    }

    public async Task<ContactDTO> GetContactById(int id)
    {
        var model = await GetContactModelById(id);
        if (model == null)
        {
            return null;
        }

        return new ContactDTO
        {
            FirstName = model.FirstName,
            Surname = model.Surname,
            Email = model.Email,
            Company = model.Company,
        };
    }

    public async Task<int> SaveOrUpdateContact(ContactDTO dto)
    {
        Contact contact;
        if (dto.Id == default)
        {
            // Create a new Contact
            contact = new Contact
            {
                Email = dto.Email,
                Company = dto.Company,
                FirstName = dto.FirstName,
                Surname = dto.Surname,
                PhoneNumbers = dto.PhoneNumbers.Select(phoneNumber => new PhoneNumber
                {
                    ContactNumber = phoneNumber
                }).ToList()
            };

            await db.Contacts.AddAsync(contact);
        }
        else
        {
            contact = await GetContactModelById(dto.Id);
            contact.FirstName = dto.FirstName;
            contact.Surname = dto.Surname;
            contact.Email = dto.Email;
            contact.Company = dto.Company;
            db.Contacts.Update(contact);
        }

        await db.SaveChangesAsync();
        return contact.Id;
    }
}

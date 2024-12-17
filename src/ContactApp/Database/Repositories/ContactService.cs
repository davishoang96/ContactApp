using ContactApp.DTO;
using ContactApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Database.Repositories;

public interface IContactService
{
    Task<int> SaveOrUpdateContact(ContactDTO contactDTO);
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
        var model = await db.Contacts.SingleOrDefaultAsync(s => s.Id == id);
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

    public async Task<int> SaveOrUpdateContact(ContactDTO contactDTO)
    {
        Contact contact = null;
        if (contactDTO.Id == default)
        {
            // Create a new Contact
            contact = new Contact
            {
                Email = contactDTO.Email,
                Company = contactDTO.Company,
                FirstName = contactDTO.FirstName,
                Surname = contactDTO.Surname,
                PhoneNumbers = contactDTO.PhoneNumbers.Select(phoneNumber => new PhoneNumber
                {
                    ContactNumber = phoneNumber
                }).ToList()
            };

            db.Contacts.Add(contact);
        }
        else
        {
            contact = await GetContactModelById(contactDTO.Id);
            contact.FirstName = contactDTO.FirstName;
            contact.Surname = contactDTO.Surname;
            contact.Email = contactDTO.Email;
            contact.Company = contactDTO.Company;

            db.Contacts.Update(contact);
        }

        return await db.SaveChangesAsync();
    }

}

using ContactApp.DTO;

namespace ContactApp.Services;

public interface IContactService
{
    Task<int> SaveOrUpdateContact(ContactDTO dto);
    Task<ContactDTO> GetContactById(int id);
    Task<IEnumerable<ContactDTO>> GetAllContacts();
}

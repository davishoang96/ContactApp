using ContactApp.Database;
using ContactApp.Database.Repositories;
using ContactApp.DTO;
using ContactApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Test;

public class ContactServiceTest : BaseDataContextTest
{
    [Fact]
    public async Task GetAllContactsOK()
    {
        // Arrange
        var service = new ContactService(new DatabaseContext(Options));
        
        using (var context = new DatabaseContext(Options))
        {
            for (var i = 0; i < 10; i++)
            {
                var contact = new Contact
                {
                    FirstName = $"Name[{i}]",
                    Surname = "Hoang",
                    Email = $"email{i}@email.com",
                    Company = $"Company-[{i}]",
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber
                        {
                            ContactNumber = $"11{i} 22{i} 33{i}"
                        }
                    }
                };
                
                await context.Contacts.AddAsync(contact);
            }
            
            await context.SaveChangesAsync();
        }
        
        // Act
        var result = await service.GetAllContacts();
        
        // Arrange
        Assert.NotNull(result); 
    }
    
    [Fact]
    public async Task SaveContactOK()
    {
        // Arrange
        var service = new ContactService(new DatabaseContext(Options));
        var dto = new ContactDTO
        {
            Company = "Zoodata",
            Email = "vhoang@zoodata.com.au",
            FirstName = "Viet",
            Surname = "Hoang",
            PhoneNumbers = new List<string>
            {
                "000 100 100",
                "010 123 125",
            }
        };
        
        // Act
        var result = await service.SaveOrUpdateContact(dto);

        // Assert
        Assert.True(result > 0);
        using (var context = new DatabaseContext(Options))
        {
            Assert.True(context.PhoneNumbers.Any());
            var model = context.Contacts.Include(s=>s.PhoneNumbers).SingleOrDefault(s => s.Id == result);
            Assert.NotNull(model);
            Assert.Equal(dto.Company, model.Company);
            Assert.Equal(dto.FirstName, model.FirstName);
            Assert.Equal(dto.Surname, model.Surname);
        }
    }
}

using ContactApp.Database;
using ContactApp.Database.Repositories;
using ContactApp.DTO;

namespace ContactApp.Test;

public class ContactServiceTest : BaseDataContextTest
{
    [Fact]
    public async Task SaveContactOK()
    {
        // Arrange
        var service = new ContactService(new DatabaseContext(Options));

        // Act
        var result = await service.SaveOrUpdateContact(new ContactDTO
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
        });

        // Assert
        Assert.True(result > 0);

        using (var context = new DatabaseContext(Options))
        {
            Assert.True(context.PhoneNumbers.Any());
            var m = context.Contacts.FirstOrDefault();
            Assert.NotNull(m);
        }
    }
}

namespace ContactApp.Repository.Models;

public class Contact : BaseModel
{
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string? Company { get; set; }
    public string? Email { get; set; }
    public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
}
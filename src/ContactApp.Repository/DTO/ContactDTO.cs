namespace ContactApp.DTO;

public class ContactDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string? Company { get; set; }
    public string Email { get; set; }
    public List<string> PhoneNumbers { get; set; }
}

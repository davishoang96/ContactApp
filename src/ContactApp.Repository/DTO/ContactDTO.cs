using System.ComponentModel.DataAnnotations;

namespace ContactApp.DTO;

public class ContactDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Surname is required.")]
    public string Surname { get; set; }
    public string? Company { get; set; }
    public string? Email { get; set; }

    [Required(ErrorMessage = "Phone numbers are required.")]
    public List<string> PhoneNumbers { get; set; }
}

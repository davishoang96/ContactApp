namespace ContactApp.Repository.Models;

public class PhoneNumber : BaseModel
{
    public int Contact_Id { get; set; }
    public string ContactNumber { get; set; }
    public Contact Contact { get; set; }
}

using ContactApp.DTO;
using ContactApp.Repository.Models;
using ContactApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactApp.Pages;

public class CreateContact : PageModel
{
    private readonly IContactService _contactService;
    public CreateContact(IContactService contactService)
    {
        _contactService = contactService;
    }
    
    [BindProperty]
    public ContactDTO Dto { get; set; }
    
    [BindProperty]
    public IEnumerable<string> PhoneNumbers { get; set; }
    
    public void OnGet()
    {
        Dto = new ContactDTO();
        Dto.PhoneNumbers = new List<string>();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        var contact = new ContactDTO
        {
            FirstName = Dto.FirstName,
            Surname = Dto.Surname,
            Email = Dto.Email,
            Company = Dto.Company,
            PhoneNumbers = Dto.PhoneNumbers
        };

        // Save to database
        await _contactService.SaveOrUpdateContact(contact);
        
        // Redirect to a confirmation page or list page
        return RedirectToPage("/Index");
    }
}
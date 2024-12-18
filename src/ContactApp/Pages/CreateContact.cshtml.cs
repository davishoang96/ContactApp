using ContactApp.DTO;
using ContactApp.Repository.Models;
using ContactApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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
    [Required(ErrorMessage = "Phone numbers are required.")]
    public IEnumerable<string> PhoneNumbers { get; set; }

    [FromQuery(Name = "id")]
    public int? ContactId { get; set; }

    public async Task OnGetAsync()
    {
        if (ContactId.HasValue)
        {
            // Load the contact for editing
            var contact = await _contactService.GetContactById(ContactId.Value);
            if (contact == null)
            {
                return;
            }

            Dto = contact;
        }
        else
        {
            Dto = new ContactDTO()
            {
                PhoneNumbers = new List<string>
                {
                    ""
                }
            };
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var invalidPhoneNumbers = new List<string>();
        foreach (var phone in Dto.PhoneNumbers)
        {
            if (string.IsNullOrWhiteSpace(phone) || phone.Length > 10 || !phone.All(char.IsDigit))
            {
                ModelState.AddModelError("Dto.PhoneNumbers", "Invalid phone number: " + phone);
                invalidPhoneNumbers.Add(phone);
            }
        }

        if (invalidPhoneNumbers.Any())
        {
            return Page();
        }

        var contact = new ContactDTO
        {
            Id = ContactId ?? 0,
            FirstName = Dto.FirstName,
            Surname = Dto.Surname,
            Email = Dto.Email,
            Company = Dto.Company,
            PhoneNumbers = Dto.PhoneNumbers
        };

        var result = await _contactService.SaveOrUpdateContact(contact);

        return RedirectToPage("/Index");
    }
}
using ContactApp.DTO;
using ContactApp.Repository.Models;
using ContactApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IContactService _contactService;

        public IndexModel(ILogger<IndexModel> logger, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }
        
        [BindProperty]
        public IEnumerable<ContactDTO> Dto { get; set; } = new List<ContactDTO>();

        public async Task OnGetAsync()
        {
            var result = await _contactService.GetAllContacts();
            if (result is null)
            {
                _logger.LogInformation("No Contacts Found");
            }
            else
            {
                Dto = result;
            }
        }
    }
}

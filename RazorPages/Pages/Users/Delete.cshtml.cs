using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Services.Interfaces;

namespace RazorPages.Pages.Users
{
    public class DeleteModel : PageModel
    {
        public IUsersServiceAsync Service {get;}

        public DeleteModel(IUsersServiceAsync service)
        {
            Service = service;
        }

        public User Entity {get; set;}

        [BindProperty(SupportsGet = true)]
        public int Id {get; set;}

        public async Task<IActionResult> OnGetAsync(int id) {
            Entity = await Service.ReadAsync(id);
            if(Entity == null)
                return NotFound();
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync() {
            await Service.DeleteAsync(Id);
            return RedirectToPage("./Index");
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Services.Interfaces;

namespace RazorPages.Pages.Users
{
    public class AddOrEditModel : PageModel
    {
        public IUsersServiceAsync Service {get;}

        public AddOrEditModel(IUsersServiceAsync service)
        {
            Service = service;
        }

        [BindProperty]
        public User Entity {get; set;}

        [BindProperty(SupportsGet = true)]
        public int? Id {get; set;}

        public async Task OnGetAsync() {
            if(Id == null || Id == 0)
                return;
            Entity = await Service.ReadAsync(Id.Value);
        }
        
        public async Task<IActionResult> OnPostAsync() {
            System.Console.WriteLine("abc");

            if(Id == null || Id == 0) {
                if(await Service.ReadByLoginAsync(Entity.Login) != null)
                {
                    ModelState.AddModelError(nameof(Entity.Login), "Login already exists!");
                    return Page();
                }
                await Service.CreateAsync(Entity);
            }
            else
            {
                var users = await Service.ReadAsync();
                if(users.Where(x => x.Id != Entity.Id).Any(x => x.Login == Entity.Login))
                {
                    //return BadRequest(ModelState);
                    ModelState.AddModelError(nameof(Entity.Login), "Login already exists!");
                    return Page();
                }
                await Service.UpdateAsync(Id.Value, Entity);
            }


            return RedirectToPage("./Index");
            //return RedirectToPage("/Users/Index");
        }
    }
}
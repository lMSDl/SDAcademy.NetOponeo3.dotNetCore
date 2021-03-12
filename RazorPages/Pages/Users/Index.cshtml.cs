using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace RazorPages.Pages.Users
{
    public class IndexModel : PageModel
    {
        public IUsersServiceAsync Service {get;}

        public IndexModel(IUsersServiceAsync service)
        {
            Service = service;
        }

        
    }
}
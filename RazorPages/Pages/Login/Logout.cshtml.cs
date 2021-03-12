using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace RazorPages.Pages.Login
{
    public class LogoutModel : PageModel
    {
            public async Task<IActionResult> OnGetAsync() {
                await HttpContext.SignOutAsync();
                return RedirectToPage("/Index");
            }
    }
}
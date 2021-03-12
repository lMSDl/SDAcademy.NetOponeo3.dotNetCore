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
    public class IndexModel : PageModel
    {
        public IUsersServiceAsync Service {get;}

        public IndexModel(IUsersServiceAsync service)
        {
            Service = service;
        }

        [BindProperty]
        public string ReturnUrl {get; set;}
        [BindProperty]
        public string Login {get; set;}
        [BindProperty]
        public string Password {get; set;}

        public async Task<IActionResult> OnPostAsync() {
            var user = await Service.ReadByLoginAsync(Login);

            if(user == null || user.Password != Password) {
                ModelState.AddModelError(nameof(Login), "Invalid credentails");
                return Page();
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(user.Role.ToString().Split(',').Select(x => new Claim(ClaimTypes.Role, x.Trim())));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

            if(!Url.IsLocalUrl(ReturnUrl))
                ReturnUrl = Url.Content("/");
            return Redirect(ReturnUrl);
        }
    }
}
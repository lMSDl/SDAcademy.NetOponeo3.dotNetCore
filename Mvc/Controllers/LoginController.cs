using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace Mvc.Controllers
{

    [Authorize]
    public class LoginController : Controller
    {
        private IUsersServiceAsync Service {get;}

        public LoginController(IUsersServiceAsync service)
        {
            Service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            var user = await Service.ReadAsync(1);
            System.Console.WriteLine($"{user.Login} {user.Password} {user.Role}");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string login, string password, string returnUrl) {
            var user = await Service.ReadByLoginAsync(login);

            if(user == null || user.Password != password) {
                ModelState.AddModelError(nameof(login), "Invalid credentails");
                return View();
            }

            var claims = new List<Claim> {
                //new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("Key", Program.Key)
            };

            claims.AddRange(user.Role.ToString().Split(',').Select(x => new Claim(ClaimTypes.Role, x.Trim())));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

            if(!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.Content("/");
            return Redirect(returnUrl);
        }
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
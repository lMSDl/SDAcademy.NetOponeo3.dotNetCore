using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace Mvc.Controllers
{
    public class UsersController : Controller
    {
        private ICrudServiceAsync<User> Service {get;}

        public UsersController(ICrudServiceAsync<User> service)
        {
            Service = service;
        }

        public async Task<IActionResult> Index(string search, Roles? roles) {
            var users = await Service.ReadAsync();

            if(!string.IsNullOrEmpty(search)) {
                users = users.Where(x => x.Login.Contains(search, System.StringComparison.InvariantCultureIgnoreCase));
            }
            if(roles.HasValue)
                users = users.Where(x => x.Role.HasFlag(roles.Value));

            return View(users);
        }

        public async Task<string> Search(string id, int limit = int.MaxValue) {
            // if(id == null)
            //     return await Index();

            var users = await Service.ReadAsync();
            return string.Join(", ", users.Take(limit).Select(x => x.Login).Where(x => x.Contains(id)));
        }
    }
}
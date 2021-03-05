using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace Mvc.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class UsersController : Controller
    {
        private IUsersServiceAsync Service {get;}

        public UsersController(IUsersServiceAsync service)
        {
            Service = service;
        }

        [Authorize(Roles = nameof(Roles.Read))]
        public async Task<IActionResult> Index(string search, Roles? roles) {
            var users = await Service.FindAsync(search, roles);

            return View(users);
        }

        [Authorize(Roles = nameof(Roles.Delete))]
        public async Task<IActionResult> Delete(int? id) {
            if(!id.HasValue)
                return BadRequest();

            var item = await Service.ReadAsync(id.Value);
            if(item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Delete))]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int? id) {
            if(!id.HasValue)
                return BadRequest();

            await Service.DeleteAsync(id.Value);

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = nameof(Roles.Create))]
            public IActionResult Add() {
            return View(new User());
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Create))]
        public async Task<IActionResult> Add(User user) {
            if(!ModelState.IsValid) {
                return View(user);
            }

            var users = await Service.ReadAsync();
            if(users.Any(x => x.Login == user.Login))
            {
                ModelState.AddModelError(nameof(user.Login), "Login already exists!");
                return View(user);
            }

            await Service.CreateAsync(user);
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = nameof(Roles.Update))]
            public async Task<IActionResult> Edit(int? id) {
            if(!id.HasValue)
                return BadRequest();

            var item = await Service.ReadAsync(id.Value);
            if(item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Update))]
        public async Task<IActionResult> Edit(int id, User user) {
            if(!ModelState.IsValid) {
                return View(user);
            }

            var users = await Service.ReadAsync();
            if(users.Where(x => x.Id != id).Any(x => x.Login == user.Login))
            {
                //return BadRequest(ModelState);
                ModelState.AddModelError(nameof(user.Login), "Login already exists!");
                return View(user);
            }

            await Service.UpdateAsync(id, user);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = nameof(Roles.Read))]
        public async Task<string> Search(string id, int limit = int.MaxValue) {
            // if(id == null)
            //     return await Index();

            var users = await Service.ReadAsync();
            return string.Join(", ", users.Take(limit).Select(x => x.Login).Where(x => x.Contains(id)));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace Mvc.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public abstract class BaseController<T, TService> : Controller where TService : ICrudServiceAsync<T> where T : Entity
    {
        protected TService Service {get;}

        public BaseController(TService service)
        {
            Service = service;
        }


        [Authorize(Roles = nameof(Roles.Delete))]
        public virtual async Task<IActionResult> Delete(int? id) {
            if(!id.HasValue)
                return BadRequest();

            var item = await Service.ReadAsync(id.Value);
            if(item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Delete))]
        public virtual async Task<IActionResult> Delete(int id) {
            await Service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
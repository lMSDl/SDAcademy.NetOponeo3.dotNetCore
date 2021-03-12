using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace Mvc.Controllers
{
    public class TiresController : BaseController<Tire, ICrudServiceAsync<Tire>>
    {
        public TiresController(ICrudServiceAsync<Tire> service) : base(service)
        {
        }

        public async Task<IActionResult> Index(TireSeason? season) {
            var tires = await Service.ReadAsync();
            //string.Join(", ", users.Select(x => x.Login));
            
            if(season.HasValue)
                tires = tires.Where(x => x.Season.Equals(season));
            
            return View(tires);
        }
        
        public IActionResult Add() {
            return View(new Tire());
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(Tire tire) 
        {
            if(!ModelState.IsValid) {
                return View(tire);
            }

            var users = await Service.ReadAsync();
            if(users.Any(x => x.Producer == tire.Producer))
            {
                ModelState.AddModelError(nameof(tire.Producer), "Producer already exists!");
                return View(tire);
            }

            await Service.CreateAsync(tire);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id) {
            if(!id.HasValue)
                return BadRequest();

            var item = await Service.ReadAsync(id.Value);
            if(item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Tire tire) {
            if(!ModelState.IsValid) {
                return View(tire);
            }
            
            var tires = await Service.ReadAsync();
            if(tires.Where(x => x.Id != id).Any(x => x.Producer == tire.Producer))
            {
                //return BadRequest(ModelState);
                ModelState.AddModelError(nameof(tire.Producer), "Producer already exist");
                return View(tire);
            }
            
            await Service.UpdateAsync(id, tire);

            return RedirectToAction(nameof(Index));
        }
        
        public async Task<string> Search(string id, int limit = int.MaxValue) {
            // if(id == null)
            //     return await Index();

            var users = await Service.ReadAsync();
            return string.Join(", ", users.Take(limit).Select(x => x.Producer.Name).Where(x => x.Contains(id)));
        }
    }
}
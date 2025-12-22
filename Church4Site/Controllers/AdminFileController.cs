using Church4Site.Entities;
using Church4Site.Models;
using Church4Site.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Church4Site.Controllers
{
    public class AdminFileController : Controller
    {

        private readonly Church4DbContext _context;
        public AdminFileController(Church4DbContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            var vm = new EventsDataViewModel
            {
                NewEvent = new EventData(),
                Eventslst = _context.Events.ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitEvent(EventsDataViewModel event1) 
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(event1.NewEvent);
                await _context.SaveChangesAsync();

                var vm = new EventsDataViewModel
                {
                    NewEvent = new EventData(),
                    Eventslst = _context.Events.ToList()
                };
                return View("AdminPage", vm);
            }
            return Ok("invalid input");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EventData ev)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Update(ev);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminPage");
            }
            return View(ev);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var item = await _context.Events.FindAsync(id);
            if (item != null)
            {
                _context.Events.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AdminPage");
        }
    }
}

using Church4Site.Entities;
using Church4Site.Models;
using Church4Site.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Church4Site.Controllers
{
    public class AdminFileController : Controller
    {

        private readonly Church4DbContext _context;
        private readonly IMainServices _mainServices;
        private readonly IWebHostEnvironment _environment;
        public AdminFileController(Church4DbContext context, IMainServices mainServices, IWebHostEnvironment environment)
        {
            _context = context;
            _mainServices = mainServices;
            _environment = environment;
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminPage()
        {
            var vm = new AdminViewModel
            {
                NewEvent = new EventData(),
                EventsLst = await _context.Events.ToListAsync(),
                MessagesLst = await _context.UserMessages.Select(x => new UserMessage
                {
                    Id = x.Id,
                    MessageFrom = x.MessageFrom,
                    IsApproved = x.IsApproved,
                    Title = x.Title,
                    UserId = x.UserId,
                    ImageUrl = x.ImageUrl,
                    Users = new User
                    {
                        Username = x.Users.Username
                    }
                }).ToListAsync(),

                NewTeamMember = new TeamMember(),
                TeamMembersLst = await _context.TeamMembers.ToListAsync(),
                NewUser = new User(),
                UserLst = await _context.Users.ToListAsync(),
                contactFormLst = await _context.ContactForms.ToListAsync()
            };
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> SubmitEvent(EventsDataViewModel event1, IFormFile imageFile)
        {

            string imgPath = await _mainServices.CreateImageAsync(imageFile, "Events");
            event1.NewEvent.ImageUrl = imgPath;

            if (ModelState.IsValid)
            {
                _context.Events.Add(event1.NewEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminPage");
            }
            return BadRequest("Validation failed or file missing.");
        }

        [HttpPost]
        public async Task<IActionResult> test(EventsDataViewModel evnt, IFormFile imageFile) 
        {
            string imgPath = await _mainServices.CreateImageAsync(imageFile, "Events");
            evnt.NewEvent.ImageUrl = imgPath;

            if (ModelState.IsValid) 
            {
                _context.Events.Add(evnt.NewEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminPage");
            }
            return BadRequest("this is a problem");
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



        

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEventConfirm(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID received");
                }

                var item = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);

                if (item != null)
                {
                    // OPTIONAL: Delete the actual physical file from the server too!
                    if (!string.IsNullOrEmpty(item.ImageUrl))
                    {
                        var filePath = Path.Combine(_environment.WebRootPath, item.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    _context.Events.Remove(item);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("AdminPage");
            }
            catch (Exception ex)
            {
                // This will print the REAL error to the screen
                var realError = ex.InnerException?.Message ?? ex.Message;
                return Content($"The real error is: {realError}");
            }
        }



        public async Task<IActionResult> DeleteTestimonies(int id)
        {
            var item = await _context.UserMessages.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }



        [HttpPost, ActionName("DeleteTestimonies")]
        public async Task<IActionResult> DeleteTestimoniesConfirm(int id)
        {
            var item = await _context.UserMessages.FindAsync(id);
            if (item != null)
            {
                _context.UserMessages.Remove(item);
                await _context.SaveChangesAsync();
            }
            else { return BadRequest(id); }
            return RedirectToAction("AdminPage");
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateTeamMember(TeamMember NewTeamMember,[FromForm] IFormFile imageFile)
        {
            try
            {
                NewTeamMember.ImageUrl = await _mainServices.CreateImageAsync(imageFile, "TeamMemberPhoto");
                if (ModelState.IsValid)
                {
                    bool EmailExists = await _context.Users.AnyAsync(u => u.Username == NewTeamMember.Email);
                    if (!EmailExists)
                    {
                        TempData["EmailNotFound"] = true;
                        return RedirectToAction("AdminPage");
                    }
                    var name = NewTeamMember.Email;
                    Guid userid = await _context.Users.Where(x => x.Username == name).Select(x => x.Id).FirstOrDefaultAsync();

                    NewTeamMember.UserId = userid;

                    _context.TeamMembers.Add(NewTeamMember);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AdminPage");
                }
                return BadRequest("Invalid Data");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> DeleteTeamMember(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == email);

                var teammember = await _context.TeamMembers.FirstOrDefaultAsync(x => x.UserId == user.Id);
                if (teammember != null)
                {
                    _context.TeamMembers.Remove(teammember);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AdminPage");

                }
                return BadRequest("Team member not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> IsMessageApproved(int id) 
        {
            try
            {
                var message = await _context.UserMessages.FirstOrDefaultAsync(x => x.Id == id);

                if (message == null) return BadRequest("Error - cant locate message, please contact amir the developer");

                /*reverse bool state*/
                message.IsApproved = !message.IsApproved;

                _context.UserMessages.Update(message);
                await _context.SaveChangesAsync();

                return Json(new { success = true, isApproved = message.IsApproved });
            }
            catch
            {
                return BadRequest("problem please contact amir");
            }
        }

        public async Task<IActionResult> Test() 
        {
            var data = new AdminViewModel
            {
                NewEvent = new EventData()

            };
            return View(data);
        }
    }
}

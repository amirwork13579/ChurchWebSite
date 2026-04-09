using Church4Site.Entities;
using Church4Site.Models;
using Church4Site.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Church4Site.Controllers
{
    public class MainController : Controller
    {
        private readonly IAuthService _authService; 
        private readonly Church4DbContext _context;
        private readonly IMainServices _mainServices;

        public MainController(IAuthService authService, Church4DbContext context, IMainServices mainService)
        {
            _authService = authService;
            _context = context;
            _mainServices = mainService;
        }

        public async Task<IActionResult> MainPage()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }
        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Calendar()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }
        public IActionResult SunDays()
        {
            return View();
        }
        public IActionResult ContactForm()
        {
            var item = new ContactForm();
            return View(item);
        }
        public IActionResult OurBeliefs()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            Response.Cookies.Delete("AuthToken");
            return Redirect("LogIn");
        }

        public async Task<IActionResult> OurStaff()
        {
            var staff = await _context.TeamMembers.Where(u => u.IsDisplayed == true).ToListAsync();
            return View(staff);
        }
        public async Task<IActionResult> Testimonies()
        {

            var messages = new UserMessageViewModel
            {
                NewMessage = new UserMessage(),
                MessagesLst = await _context.UserMessages.Where(m => m.IsApproved == true).ToListAsync(),
            };

            return View(messages);
        }

        /*public async Task<IActionResult> Testimonies1()
        {
            var messages = new UserMessageViewModel
            {
                NewMessage = await _context.UserMessages
                    .Include(m => m.UserTable)
                    .FirstOrDefaultAsync(),

                MessagesLst = await _context.UserMessages
                    .Include(m => m.UserTable)
                    .ToListAsync(),

                userid = await _context.UserMessages
                    .Select(m => m.UserId)
                    .Distinct()
                    .ToListAsync()
            };

            return View(messages);
        }*/

        public async Task<IActionResult> EditTestimonie(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound("bad");
            }
            var message = await _context.UserMessages.FirstOrDefaultAsync(m => m.Id == id);
            return View(message);
        }



        [HttpPost, ActionName("EditTestimonieConfirm")]
        public async Task<IActionResult> EditTestimonieConfirm(UserMessage Msg)
        {
            if (Msg.Id == 0 || Msg.Id == null)
            {
                return BadRequest("The ID was not provided. Update failed.");
            }

            if (ModelState.IsValid)
            {
                _context.UserMessages.Update(Msg);
                await _context.SaveChangesAsync();
                return RedirectToAction("Testimonies");
            }
            return BadRequest("model state not valid");
        }



        [HttpPost]
        public async Task<IActionResult> PostTestimonie(UserMessage message, [FromForm] IFormFile FormFile)
        {
            message.MessageDate = DateTime.UtcNow;

            try
            {
                var imageUrl = await _mainServices.CreateImageAsync(FormFile, "userPhoto");
                message.ImageUrl = imageUrl;

                if (imageUrl == null || imageUrl == string.Empty)
                {
                    message.ImageUrl = "/css/Images/DefaultUser.jpg";
                }

                ModelState.Remove("ImageUrl");
                ModelState.Remove("UserId");
                ModelState.Remove("FormFile");

                if (ModelState.IsValid)
                {
                    var idString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (string.IsNullOrEmpty(idString))
                    {
                        message.UserId = null;

                        _context.UserMessages.Add(message);
                        await _context.SaveChangesAsync();

                        return Ok();
                    }

                    // Convert the string GUID from Identity to a C# Guid object
                    message.UserId = Guid.Parse(idString);

                    _context.UserMessages.Add(message);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                return BadRequest("Invalid Data");
            }
            catch (Exception ex)
            {
                // Check the InnerException for the actual SQL error
                var error = ex.InnerException?.Message ?? ex.Message;
                return BadRequest($"Database Error: {error}");
            }
        }




        [HttpPost, ActionName("DeleteTestimoniesConfirm")]
        public async Task<IActionResult> DeleteTestimoniesConfirm(int id)
        {
            var item = await _context.UserMessages.FindAsync(id);

            string photoPath = item.ImageUrl;

            if (item != null)
            {
                _context.UserMessages.Remove(item);
                await _context.SaveChangesAsync();

                _mainServices.DeleteServerPhoto(photoPath);
            }
            else { return BadRequest(id); }
            return RedirectToAction("Testimonies");
        }




        [HttpPost]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("User Already Exists");
            }
            return Redirect("MainPage");
        }




        [HttpPost]
        public async Task<ActionResult<string>> LogIn(UserDto request)
        {
            var token = await _authService.LoginAsync(request);
            if (token == null)
            {
                TempData["LoginError"] = "Invalid email or password.";
                return View();
            }

            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,   // prevents JavaScript access
                Secure = true,     // only over HTTPS
                SameSite = SameSiteMode.Strict
            });

            ViewBag.Token = "hello hacker";

            return Redirect("MainPage");
        }


        public string GenRefToken()
        {
            var randnum = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randnum);
            return Convert.ToBase64String(randnum);
        }



        public async Task<IActionResult> Test()
        {
            var items = await _context.Events.ToListAsync();

            var testimonieLst = await _context.UserMessages.ToListAsync();

            return View(testimonieLst);
        }

        [HttpPost]
        public async Task<IActionResult> ContactFormSubmit(ContactForm contactForm)
        {
            contactForm.SentDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.ContactForms.Add(contactForm);
                await _context.SaveChangesAsync();
                return Ok();
                //return PartialView("_SuccessMessage", contactForm.Sender);
            }
            return BadRequest("dam");
        }
    }
}

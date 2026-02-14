using Church4Site.Entities;
using Church4Site.Models;
using Church4Site.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Church4Site.Controllers
{
    public class MainController : Controller
    {
        private readonly IAuthService _authService;
        private readonly Church4DbContext _context;
        public MainController(IAuthService authService, Church4DbContext context)
        {
            _authService = authService;
            _context = context;
        }


        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            return View();
        }
        public IActionResult Youth()
        {
            return View();
        }
        public async Task<IActionResult> Testimonies1() 
        {

            var messages = new UserMessageViewModel 
            {
                NewMessage = _context.UserMessages.FirstOrDefault(),
                MessagesLst = await _context.UserMessages.ToListAsync(),
                userid = await _context.UserMessages.Select(m => m.UserId).Distinct().ToListAsync()
            };

            return View(messages);
        }

        public async Task<IActionResult> Testimonies()
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
        }

        public async Task<IActionResult> EditTestimonie(int id) 
        {
            if(id == null || id == 0) 
            {
                return NotFound("bad");
            }
            var message = await _context.UserMessages.FirstOrDefaultAsync(m => m.Id == id);    
            return View(message);
        }

        [HttpPost, ActionName("EditTestimonieConfirm")]
        public async Task<IActionResult> EditTestimonieConfirm(UserMessage Msg)
        {
            if (Msg.Id == 0)
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


        [Authorize]
        [HttpPost]
        public IActionResult PostTestimonie(UserMessage message) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var idString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (string.IsNullOrEmpty(idString))
                    {
                        return BadRequest("You must be logged in to post.");
                    }
                    

                    // Convert the string GUID from Identity to a C# Guid object
                    message.UserId = Guid.Parse(idString);

                    _context.UserMessages.Add(message);
                    _context.SaveChanges();

                    return RedirectToAction("Testimonies");
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


        public async Task<IActionResult> MainPage() 
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }
        public IActionResult LogOut() 
        {
            Response.Cookies.Delete("AuthToken");
            return Redirect("LogIn");
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
                return BadRequest("Wrong UserName or PassWord");
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
        public IActionResult SunDays()
        {
            return View();
        }

    }
}

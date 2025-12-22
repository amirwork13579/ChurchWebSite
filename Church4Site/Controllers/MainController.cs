using Church4Site.Entities;
using Church4Site.Models;
using Church4Site.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Church4Site.Controllers
{
    public class MainController: Controller 
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

            ViewBag.Token = token;

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
            var eventItem = await _context.Events.ToListAsync();
            return View(eventItem);
        }

        public IActionResult SunDays()
        {
            return View();
        }

    }
}

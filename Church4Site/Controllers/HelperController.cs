using Church4Site.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Church4Site.Controllers
{
    [Route("Helper")]
    public class HelperController :Controller
    {
        private readonly Church4DbContext _context;
        public HelperController(Church4DbContext context) 
        {
            _context = context;
        }


        /*httpget so js can get it with a get request*/
        [HttpGet("GetUserEmailAsync")]
        public async Task<IActionResult> GetUserEmailAsync(string email)
        {
            bool isEmailExists = false;

            List<string> emails = await _context.Users.Select(u => u.Username).ToListAsync();

            var useEmail = await _context.Users.AnyAsync(u => u.Username == email);

            if (emails.Contains(email)) 
            {
                isEmailExists = true;
            }
            return Json(isEmailExists);
        }
    }
}

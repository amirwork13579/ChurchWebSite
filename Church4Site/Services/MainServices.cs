using Church4Site.Entities;
using Church4Site.Migrations;
using Church4Site.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Church4Site.Services
{
    public class MainServices : IMainServices
    {
        private readonly IWebHostEnvironment _environment;
        private readonly Church4DbContext _context;

        public MainServices(IWebHostEnvironment environment, Church4DbContext context)
        {
            _environment = environment;
            _context = context;
        }


        public async Task<string> CreateImageAsync(IFormFile imageFile, string location)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // 1. Clean the locationName to ensure it's lowercase and safe for URLs
                string folderName = location.ToLower().Trim();

                // 2. Build the folder path dynamically (e.g., uploads/teams or uploads/events)
                // Path.Combine handles the slashes correctly for different operating systems
                string folder = Path.Combine(_environment.WebRootPath, "uploads", folderName);

                // 3. Ensure the specific directory exists
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // 4. Create a unique filename
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string fullPath = Path.Combine(folder, fileName);

                // 5. Save the physical file
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // 6. Return the Web URL for the database
                // We use forward slashes here because this is for a Web URL, not a file path
                return $"/uploads/{folderName}/{fileName}";
            }

            return null;
        }




        [HttpPost]
        public void DeleteServerPhoto(string path)
        {
            // 1. Get the physical path to the file
            // Example: member.ImageUrl is "/uploads/user123.jpg"
            if (!string.IsNullOrEmpty(path) && !path.Contains("DefaultUser.jpg"))
            {
                // Path.Combine handles the slashes correctly for Windows or Linux
                var filePath = Path.Combine(_environment.WebRootPath, path.TrimStart('/'));

                // 2. Check if the file actually exists before trying to delete
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // 3. Delete the record from the Database


        }


        public async Task<List<string>> getAllUserEmails() 
        {
            List<string> emailLst = await _context.Users.Select(u => u.Username).ToListAsync();
            return emailLst;
        }
    }
}

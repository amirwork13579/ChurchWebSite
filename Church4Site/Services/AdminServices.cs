using Church4Site.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Church4Site.Services
{
    public class AdminServices
    {
        private readonly Church4DbContext _context;
        public AdminServices(Church4DbContext context)
        {
            _context = context;
        }


        

    }
}

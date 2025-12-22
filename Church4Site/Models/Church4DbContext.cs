using Church4Site.Entities;
using Microsoft.EntityFrameworkCore;

namespace Church4Site.Models
{
    public class Church4DbContext(DbContextOptions<Church4DbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EventData> Events { get; set; }
    }
}

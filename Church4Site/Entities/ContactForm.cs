using Church4Site.Migrations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Church4Site.Entities
{
    public class ContactForm
    {
        [Key]
        public int Id { get; set; } 
        public string Sender { get; set; }
        public DateTime SentDate { get; set; }
        public string Email { get; set; }
        public string ?Phone { get; set; }
        public string Message { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Church4Site.Entities
{
    public class UserMessage
    {
        public int Id { get; set; }
        public string ?userName { get; set; }
        public string ?MessageFrom { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;
        public string ?Title { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        [ValidateNever]
        public Guid ?UserId { get; set; }
        [ValidateNever] /*to ensure its nulllable*/
        public string ?ImageUrl { get; set; } = string.Empty;
        public User ?Users { get; set; }

    }
}

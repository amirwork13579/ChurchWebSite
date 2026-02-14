using System.ComponentModel.DataAnnotations;

namespace Church4Site.Entities
{
    public class EventData
    {
        public int Id { get; set; }
        public string ?Name { get; set; } = string.Empty;
        public string ?Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly ?Date { get; set; } = DateOnly.MinValue;

        [DataType(DataType.Time)]
        public TimeOnly ?Time { get; set; } = TimeOnly.MinValue;
        public string ?ImageUrl { get; set; } = string.Empty;
    }
}

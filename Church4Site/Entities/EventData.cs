namespace Church4Site.Entities
{
    public class EventData
    {
        public int Id { get; set; }
        public string ?Name { get; set; } = string.Empty;
        public string ?Description { get; set; } = string.Empty;
        public DateTime ?Date { get; set; } = DateTime.MinValue;
    }
}

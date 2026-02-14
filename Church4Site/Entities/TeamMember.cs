namespace Church4Site.Entities
{
    public class TeamMember
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ?Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ?ImageUrl { get; set; } = string.Empty;
        public bool IsDisplayed { get; set; } = false;
        public Guid UserId { get; set; }

    }
}

/*changed name to email, added name*/
using Church4Site.Entities;

namespace Church4Site.Models
{
    public class AdminViewModel
    {
        public EventData ?NewEvent { get; set; } 
        public List<EventData> ?EventsLst { get; set; }
        public List<UserMessage> ?MessagesLst { get; set; }
        public TeamMember ?NewTeamMember { get; set; }
        public List<TeamMember> ?TeamMembersLst { get; set; }
        public User ?NewUser { get; set; }
        public List<User> ?UserLst { get; set; }
    }
}

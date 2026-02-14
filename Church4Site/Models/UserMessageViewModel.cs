using Church4Site.Entities;

namespace Church4Site.Models
{
    public class UserMessageViewModel
    {
        public UserMessage ?NewMessage { get; set; }
        public List<UserMessage> ?MessagesLst { get; set; }
        public List<Guid?> ?userid { get; set; }



        public UserMessageViewModel()
        {
        }
    }
}

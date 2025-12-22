using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Church4Site.Entities
{
    public class EventsDataViewModel
    {
        public EventData ?NewEvent {  get; set; }
        public List<EventData> ?Eventslst { get; set; } 
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Events.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime DateOfEvent { get; set; }
        public string EventType { get; set; }
        public string VenueId { get; set; } = "XXXXX";
    }
}
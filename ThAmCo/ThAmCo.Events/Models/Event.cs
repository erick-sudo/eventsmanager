using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Events.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime DateOfEvent { get; set; }
        public string EventType { get; set; }
        public List<Staff> StaffAllocated { get; set; } = new List<Staff>();
        public List<GuestBooking> Guests { get; set; } = new List<GuestBooking>();
    }
}

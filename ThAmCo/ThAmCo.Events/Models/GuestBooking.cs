namespace ThAmCo.Events.Models
{
    public class GuestBooking
    {
        public int GuestBookingId { get; }
        public int GuestId { get; set; }
        public int EventId { get; set; }

        public Boolean Attended { get; set; } = false;
    }
}

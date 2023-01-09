using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models
{
    public class Staffing
    {
        [Key]
        public int StaffingId { get; }

        public int StaffId { get; set; }
        public int EventId { get; set; }
    }
}

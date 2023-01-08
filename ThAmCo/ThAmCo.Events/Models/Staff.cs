using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models
{
    public class Staff
    {
        [Required]
        public int StaffId { get; set; }

        [Required]
        public string  StaffName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}

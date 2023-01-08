using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Events.Models
{
    public class Guest
    {
        [Required]
        public int GuestId { get; set; }

        [Required]
        public string GuestName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress , ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

    }
}
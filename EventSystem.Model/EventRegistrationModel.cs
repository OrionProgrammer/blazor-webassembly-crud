using System.ComponentModel.DataAnnotations;

namespace EventSystem.Model
{
    public class EventRegistrationModel
    {
        [Required(ErrorMessage = "User Id required!")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Event Id required!")]
        public long EventId { get; set; }

        public string ReferenceNumber { get; set; }
    }
}

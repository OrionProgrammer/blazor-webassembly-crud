using System.ComponentModel.DataAnnotations;

namespace EventSystem.Domain
{
    public record EventRegistration
    {
        [Key]
        public long Id { get; set; }
        public string UserId { get; set; }
        public long EventId { get; set; }
        public string ReferenceNumber { get; set; }
    }
}

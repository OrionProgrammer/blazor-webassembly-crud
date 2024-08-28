using System.ComponentModel.DataAnnotations;

namespace EventSystem.Domain
{
    public record Event
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int SeatCount { get; set; }
        public int AttendanceCount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Model
{
    public class EventModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required(ErrorMessage = "Date is required!")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Location is required!")]
        public string Location { get; set; }

        [Display(Name = "Seats Available")]
        [Required(ErrorMessage = "Seat Count is required!")]

        public int SeatCount { get; set; }
        public int AttendanceCount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    namespace AppointmentScheduler
    {
        public class User
        {
            [Key]
            public int UserId { get; set; }
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }

            public ICollection<Appointment> Appointments { get; set; }
        }

        public class Appointment
        {
            [Key]
            public int AppointmentId { get; set; }
            [Required]
            public DateTime Date { get; set; }
            [Required]
            public string Title { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
            public int UserId { get; set; }
            public User User { get; set; }
        }
    }

}

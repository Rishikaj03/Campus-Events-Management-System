using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEventsApp.Models
{
    public class EventRegistration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

   
        [Required]
        public string StudentId { get; set; }

  
        [ForeignKey("StudentId")]
        public IdentityUser Student { get; set; }

    
        public string Status { get; set; } = "Registered";

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
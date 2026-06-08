using System.ComponentModel.DataAnnotations;

namespace CampusEventsApp.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Category { get; set; } // e.g., Workshop, Seminar

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public TimeSpan EventTime { get; set; }

        [Required]
        public string Venue { get; set; }

        public int TotalCapacity { get; set; }

        public bool IsOpen { get; set; } = true; // Status: Open/Closed

        // Navigation: Who is attending?
        public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();

        // Helper to calculate capacity left (Requested in Screen #6)
        public int CapacityLeft => TotalCapacity - Registrations.Count;
    }
}
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace FlightAlertSystem.Models
{
    public class Alert
    {
        [Key]
        public int Id { get; set; } // Auto-generated primary key

        [Required]
        public int UserId { get; set; } // The user who owns this alert

        [Required]
        public string Source { get; set; } // Source airport code (e.g., CDG)

        [Required]
        public string Destination { get; set; } // Destination airport code (e.g., JFK)

        [Required]
        public DateTime DepartureDate { get; set; } // Flight departure date and time

        [Required]
        public string Airline { get; set; } // Airline name (e.g., Air France)
        
        public string? FlightId { get; set; } // Generated automatically
        
        [Required]
        public decimal PriceThreshold { get; set; } // Maximum price the user wants to pay
    }
}
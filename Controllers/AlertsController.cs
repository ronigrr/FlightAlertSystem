using FlightAlertSystem.Data;
using FlightAlertSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightAlertSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly FlightAlertContext _context;

        public AlertsController(FlightAlertContext context)
        {
            _context = context;
        }

        // Get all alerts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAlerts()
        {
            return await _context.Alerts.ToListAsync();
        }

        // Get alert by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Alert>> GetAlertById(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);

            if (alert == null)
            {
                return NotFound();
            }

            return alert;
        }

        // Create a new alert
        [HttpPost]
        public async Task<ActionResult<Alert>> CreateAlert([FromBody] Alert alert)
        {
            ModelState.Remove(nameof(Alert.FlightId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(alert.Source) || string.IsNullOrWhiteSpace(alert.Destination))
            {
                return BadRequest("Source and Destination must be provided.");
            }

            // Generate the FlightId based on flight details
            alert.FlightId = GenerateFlightId(alert);

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlertById), new { id = alert.Id }, alert);
        }

        // Update an alert
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlert(int id, [FromBody] Alert alert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAlert = await _context.Alerts.FindAsync(id);
            if (existingAlert == null)
            {
                return NotFound();
            }

            existingAlert.UserId = alert.UserId;
            existingAlert.Source = alert.Source;
            existingAlert.Destination = alert.Destination;
            existingAlert.DepartureDate = alert.DepartureDate;
            existingAlert.Airline = alert.Airline;
            existingAlert.PriceThreshold = alert.PriceThreshold;

            if (existingAlert.Source != alert.Source ||
                existingAlert.Destination != alert.Destination ||
                existingAlert.DepartureDate != alert.DepartureDate ||
                existingAlert.Airline != alert.Airline)
            {
                existingAlert.FlightId = GenerateFlightId(existingAlert);
            }

            try
            {
                _context.Entry(existingAlert).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("A concurrency error occurred while updating the alert.");
            }

            return NoContent();
        }

        // Delete an alert
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlert(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);

            if (alert == null)
            {
                return NotFound();
            }

            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method to generate FlightId
        private string GenerateFlightId(Alert alert)
        {
            return $"{alert.Source}_{alert.Destination}_{alert.DepartureDate:yyyy-MM-ddTHH:mm:ssZ}_{alert.Airline}";
        }
    }
}
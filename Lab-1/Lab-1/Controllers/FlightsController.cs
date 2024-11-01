using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab_1.Data;
using Lab_1.Models;
using Microsoft.AspNetCore.Authorization;

namespace Lab_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly FlightManager manager;

        public FlightsController(FlightManager manager)
        {
            this.manager = manager;
        }

        // POST: api/Flights
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Flight>> AddFlight([FromBody] Flight flight)
        {
            return await manager.AddFlight(flight) ? CreatedAtAction("GetFlights", new { id = flight.Number }, flight) : BadRequest("failed to add flight");
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights(DateTime? TimeFrom, DateTime? TimeTo)
        {
            var flights = await manager.GetFlights(TimeFrom, TimeTo);
            if (flights == null)
                return NotFound();
            return flights;
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            var flight = await manager.GetFlight(id);
            if (flight == null)
                return NotFound();
            return flight;
        }

        // GET: api/Flights/5
        [HttpGet("{number}/GetPassenger")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<List<PassengerDTO>>> GetPassengeerOnFlight(int number)
        {
            var passengers = await manager.GetPassengerOnFlight(number);
            if (passengers == null)
                return NotFound();
            var selectedPeople = passengers.Select(pass => new PassengerDTO(pass)).ToList();
            return selectedPeople;
        }

        // PUT: api/Flights
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutFlight([FromBody] Flight flight)
        {
            return await manager.UpdateFlight(flight) ? CreatedAtAction("GetFlights", new { id = flight.Number }, flight) : NotFound();
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            return await manager.DeletePassenger(id) ? Ok() : NotFound();
        }
    }
}

using Lab_1.Data;
using Lab_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Lab_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassengersController : ControllerBase
    {
        private readonly FlightManager manager;

        public PassengersController(FlightManager manager)
        {
            this.manager = manager;
        }

        // Admin
        [HttpPost] // POST: api/Passengers
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddPassenger([FromBody] PassengerDTO passenger)
        {
            return await manager.AddPassenger(passenger) ? CreatedAtAction("GetPassenger", new { id = passenger.ID }, passenger) : BadRequest("failed to add passenger");
        }

        // All
        [HttpGet] // GET: api/Passengers
        public async Task<ActionResult<IEnumerable<PassengerDTO>>> GetPassengers()
        {
            var passengers = await manager.GetPassengers();
            if (passengers == null)
                return NotFound();
            return passengers;
        }

        // All
        [HttpGet("{id}")] // GET: api/Passengers/5
        public async Task<ActionResult<PassengerDTO>> GetPassenger(int id)
        {
            var passenger = await manager.GetPassenger(id);
            if (passenger == null)
                return NotFound();
            return passenger;
        }

        // User
        [HttpGet("{id}/GetFlights")] // GET: api/Passengers/5/GetFlights
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetPassengerFlights(int id, bool current)
        {
            Console.WriteLine(current);
            var flights = await manager.GetPassengerFlights(id, current);
            if (flights == null)
                return NotFound();
            return flights;
        }

        // User
        [HttpPut] // PUT: api/Passengers
        [Authorize(Roles = "user")]
        public async Task<ActionResult> PutPassenger([FromBody] PassengerDTO pass)
        {
            return (await manager.UpdatePassenger(pass)) ? CreatedAtAction("GetPassenger", new { id = pass.ID }, pass) : NotFound();
        }

        // User
        [HttpPut("{id}/AddOnFlights")] // PUT: api/Passengers/5/AddOnFlights
        [Authorize(Roles = "user")]
        public async Task<ActionResult> AddPassengerOnFlight(int id, [FromBody] List<int> FlightsID)
        {
            return (await manager.AddPassengerOnFlight(id, FlightsID)) ? Ok() : NotFound();
        }

        // User
        [HttpPut("{id}/RemoveFromFlights")] // PUT: api/Passengers/5/AddOnFlight
        [Authorize(Roles = "user")]
        public async Task<ActionResult<Passenger>> RemovePassengerFromFlight(int id, [FromBody] List<int> FlightsID)
        {
            return await manager.RemovePassengerFromFlight(id, FlightsID) ? Ok() : NotFound();
        }

        // Admin
        [HttpDelete("{id}")] // DELETE: api/Passengers/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePassenger(int id)
        {
            return await manager.DeletePassenger(id) ? Ok() : NotFound();
        }
    }
}

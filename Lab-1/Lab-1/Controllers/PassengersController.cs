using Lab_1.BLL;
using Lab_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace Lab_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassengersController : ControllerBase
    {
        private readonly Manager manager;

        public PassengersController(Manager manager)
        {
            this.manager = manager;
        }

        // Admin
        [HttpPost] // POST: api/Passengers
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddPassenger([FromBody] PassengerDTO pass)
        {
            Passenger passenger = await manager.AddPassenger(new Passenger(pass));
            return passenger != null ? CreatedAtAction(nameof(GetPassenger), new { id = passenger.ID }, new PassengerDTO(passenger)) : BadRequest("failed to add passenger");
        }

        // All
        [HttpGet] // GET: api/Passengers
        public async Task<ActionResult<IEnumerable<PassengerDTO>>> GetPassengers()
        {
            var passengers = await manager.GetPassengers();
            if (passengers == null)
                return NotFound();
            return passengers.Select(pass => new PassengerDTO(pass)).ToList();
        }

        // All
        [HttpGet("{id}")] // GET: api/Passengers/5
        public async Task<ActionResult<PassengerDTO>> GetPassenger(int id)
        {
            var pass = await manager.GetPassenger(id);
            if (pass == null)
                return NotFound();
            return new PassengerDTO(pass);
        }

        // User
        [HttpGet("{id}/GetFlights")] // GET: api/Passengers/5/GetFlights
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetPassengerFlights(int id, bool current)
        {
            var flights = await manager.GetPassengerFlights(id, current);
            if (flights == null)
                return NotFound();
            return flights;
        }

        // User
        [HttpPut] // PUT: api/Passengers
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult> PutPassenger([FromBody] PassengerDTO pass)
        {
            return await manager.UpdatePassenger(new Passenger(pass)) ? Ok() : NotFound();
        }

        // User
        [HttpPut("{id}/AddOnFlights")] // PUT: api/Passengers/5/AddOnFlights
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult> AddPassengerOnFlight(int id, [FromBody] List<int> FlightsID)
        {
            return (await manager.AddPassengerOnFlight(id, FlightsID)) ? Ok() : NotFound();
        }

        // User
        [HttpPut("{id}/RemoveFromFlights")] // PUT: api/Passengers/5/AddOnFlight
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult> RemovePassengerFromFlight(int id, [FromBody] List<int> FlightsID)
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

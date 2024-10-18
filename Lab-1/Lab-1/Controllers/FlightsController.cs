using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab_1.Data;
using Lab_1.Models;

namespace Lab_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly Lab1Context _context;

        public FlightsController(Lab1Context context)
        {
            _context = context;
        }

        // POST: api/Flights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Flight>> AddFlight([FromBody] Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.Number }, flight);
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            return await _context.Flights.ToListAsync();
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return flight;
        }

        // GET: api/Flights/5
        [HttpGet("{number}/GetPassenger")]
        public async Task<ActionResult<IEnumerable<int>>> GetPassengeerOnFlight(int number)
        {
            var selectedPeople = _context.Passengers.SelectMany(p => p.Flights,
                            (p, f) => new { Passenger = p, Flight = f })
                          .Where(p => p.Flight.Number == number)
                          .Select(u => u.Passenger.ID).ToList();

            /*var test = from passenger in _context.Passengers
                                 from flight in passenger.Flights
                                 where flight.Number == number
                                 select passenger;*/

            if (selectedPeople == null)
            {
                return NotFound();
            }

            return selectedPeople;
        }

        // PUT: api/Flights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutFlight([FromBody] Flight flights)
        {
            /*if (id != flights.Number)
            {
                return BadRequest();
            }*/

            _context.Entry(flights).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightsExists(flights.Number))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var flights = await _context.Flights.FindAsync(id);
            if (flights == null)
            {
                return NotFound();
            }

            _context.Flights.Remove(flights);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlightsExists(int id)
        {
            return _context.Flights.Any(e => e.Number == id);
        }
    }
}

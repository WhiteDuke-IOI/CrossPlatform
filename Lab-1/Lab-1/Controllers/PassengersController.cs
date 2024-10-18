using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab_1.Data;
using Lab_1.Models;
using System.Drawing.Printing;
using NuGet.Versioning;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;

namespace Lab_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassengersController : ControllerBase
    {
        private readonly Lab1Context _context;

        public PassengersController(Lab1Context context)
        {
            _context = context;
        }

        // Admin
        [HttpPost] // POST: api/Passengers
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Passenger>> AddPassenger([FromBody] Passenger student)
        {
            _context.Passengers.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPassenger", new { id = student.ID }, student);
        }

        // All
        [HttpGet] // GET: api/Passengers
        public async Task<ActionResult<IEnumerable<Passenger>>> GetPassengers()
        {
            if (_context.Passengers == null)
            {
                return NotFound();
            }
            var students = await _context.Passengers.Include("Flights").ToListAsync();
            return students;
        }

        // All
        [HttpGet("{id}")] // GET: api/Passengers/5
        public async Task<ActionResult<Passenger>> GetPassenger(int id)
        {
            if (_context.Passengers == null)
            {
                return NotFound();
            }
            var student = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        // All
        [HttpGet("{id}/GetFlights")] // GET: api/Passengers/5/GetFlights
        public async Task<ActionResult<IEnumerable<Flight>>> GetPassengerFlights(int id)
        {
            if (_context.Passengers == null)
            {
                return NotFound();
            }
            var student = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            var SortedFlights = student.Flights.OrderBy(exams => exams.DepartingTime).ToList();

            return SortedFlights;
        }

        // User
        [HttpPut] // PUT: api/Passengers/5
        [Authorize(Roles = "user")]
        public async Task<IActionResult> PutPassenger([FromBody] PassengerDTO stud)
        {
            if (_context.Passengers == null)
                return NotFound();
            var student = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == stud.ID);
            if (student == null)
                return NotFound();

            _context.Passengers.Update(student.Update(stud));

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // User
        [HttpPut("{id}/AddOnFlights")] // PUT: api/Passengers/5/AddOnFlight
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AddPassengerOnFlight(int id, [FromBody] List<int> FlightsID)
        {
            if (_context.Passengers == null)
                return NotFound();
            var passenger = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == id);
            if (passenger == null)
                return NotFound();

            if (_context.Flights == null)
                return NotFound();
            foreach (var FlightID in FlightsID)
            {                
                var flight = await _context.Flights.FindAsync(FlightID);
                if (flight == null)
                    return NotFound();

                passenger.Flights.Add(flight);
            }

            _context.Passengers.Update(passenger);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPassenger", new { id = passenger.ID }, passenger);
        }

        // User
        [HttpPut("{id}/RemoveRfomFlights")] // PUT: api/Passengers/5/AddOnFlight
        [Authorize(Roles = "user")]
        public async Task<IActionResult> RemovePassengerFromFlight(int id, [FromBody] List<int> FlightsID)
        {
            if (_context.Passengers == null)
                return NotFound();
            var passenger = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == id);
            if (passenger == null)
                return NotFound();

            if (_context.Flights == null)
                return NotFound();
            foreach (var FlightID in FlightsID)
            {
                var flight = await _context.Flights.FindAsync(FlightID);
                if (flight == null)
                    return NotFound();

                passenger.Flights.Remove(flight);
            }

            _context.Passengers.Update(passenger);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPassenger", new { id = passenger.ID }, passenger);
        }

        // Admin
        [HttpDelete("{id}")] // DELETE: api/Passengers/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePassenger(int id)
        {
            var passengers = await _context.Passengers.FindAsync(id);
            if (passengers == null)
            {
                return NotFound();
            }

            _context.Passengers.Remove(passengers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PassengerExists(int id)
        {
            return _context.Passengers.Any(e => e.ID == id);
        }
    }
}

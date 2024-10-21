using Lab_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq;

namespace Lab_1.Models
{
    public class FlightManager
    {
        private readonly Lab1Context _context;

        public FlightManager(Lab1Context context) {
            _context = context;
        }

        #region Passenger
        public async Task AddPassenger(PassengerDTO passenger)
        {
            await _context.Passengers.AddAsync(new Passenger(passenger));
            await _context.SaveChangesAsync();
        }

        public async Task<List<PassengerDTO>> GetPassengers()
        {
            var passengers = await _context.Passengers.Include("Flights").ToListAsync();

            return passengers.Select(pass => new PassengerDTO(pass)).ToList();
        }

        public async Task<PassengerDTO> GetPassenger(int passengerId)
        {
            return new PassengerDTO(await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == passengerId));
        }

        public async Task<List<Flight>> GetPassengerFlights(int passengerId)
        {
            var passenger = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == passengerId);

            return passenger.Flights.OrderBy(flight => flight.DepartingTime).ToList();
        }

        public async Task<Passenger> UpdatePassenger(PassengerDTO pass)
        {
            var passenger = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == pass.ID);

            _context.Passengers.Update(passenger.Update(pass));

            _context.SaveChangesAsync();

            return passenger;
        }

        public async Task<Passenger> AddPassengerOnFlight(int id, List<int> FlightsID)
        {
            var passenger = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == id);

            foreach (var FlightID in FlightsID)
            {
                var flight = await _context.Flights.FindAsync(FlightID);

                passenger.Flights.Add(flight);
            }

            _context.Passengers.Update(passenger);
            _context.SaveChangesAsync();

            return passenger;
        }

        public async Task<Passenger> RemovePassengerFromFlight(int id, List<int> FlightsID)
        {
            var passenger = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == id);
            foreach (var FlightID in FlightsID)
            {
                var flight = await _context.Flights.FindAsync(FlightID);

                passenger.Flights.Remove(flight);
            }

            _context.Passengers.Update(passenger);
            await _context.SaveChangesAsync();

            return passenger;
        }

        public async Task DeletePassenger(int id)
        {
            var passengers = await _context.Passengers.FindAsync(id);

            _context.Passengers.Remove(passengers);
            _context.SaveChangesAsync();
        }
        #endregion

        #region Flight
        public async Task AddFlight(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Flight>> GetFlights()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<Flight> GetFlight(int id)
        {
            return await _context.Flights.FindAsync(id);
        }

        public async Task<List<PassengerDTO>> GetPassengeerOnFlight(int number)
        {
            var selectedPeople = await _context.Passengers.SelectMany(p => p.Flights,
                            (p, f) => new { Passenger = p, Flight = f })
                          .Where(p => p.Flight.Number == number)
                          .Select(u => u.Passenger).ToListAsync();

            /*var test = from passenger in _context.Passengers
                                 from flight in passenger.Flights
                                 where flight.Number == number
                                 select passenger;*/

            return selectedPeople.Select(pass => new PassengerDTO(pass)).ToList();
        }

        public async Task UpdateFlight(Flight flight)
        {
            _context.Entry(flight).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFlight(int id)
        {
            var flights = await _context.Flights.FindAsync(id);

            _context.Flights.Remove(flights);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}

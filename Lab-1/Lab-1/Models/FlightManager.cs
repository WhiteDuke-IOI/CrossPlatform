using Lab_1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_1.Models
{
    public class FlightManager
    {
        private readonly Lab1Context _context;

        public FlightManager(Lab1Context context) {
            _context = context;
        }

        #region Passenger
        public async Task<bool> AddPassenger(PassengerDTO passenger)
        {
            await _context.Passengers.AddAsync(new Passenger(passenger));
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        public async Task<List<PassengerDTO>?> GetPassengers()
        {
            //var passengers = await _context.Passengers.Include("Flights").ToListAsync();
            //return passengers.Select(pass => new PassengerDTO(pass)).ToList();
            return (await _context.Passengers.ToListAsync()).Select(pass => new PassengerDTO(pass)).ToList();
        }

        public async Task<PassengerDTO?> GetPassenger(int passengerId)
        {
            return new PassengerDTO(await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == passengerId));
        }

        // TODO добавить фильтр все или текущие
        public async Task<List<Flight>?> GetPassengerFlights(int passengerId, bool current)
        {
            var passenger = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == passengerId);
            if (current)
                return passenger.Flights.Where(flight => flight.DepartingTime.ToUniversalTime() >= DateTime.UtcNow).OrderBy(flight => flight.DepartingTime).ToList();
            else
                return passenger.Flights.OrderBy(flight => flight.DepartingTime).ToList();
        }

        public async Task<bool> UpdatePassenger(PassengerDTO pass)
        { 
            var passenger = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == pass.ID);
            if (passenger == null)
                return false;

            _context.Passengers.Update(passenger.Update(pass));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddPassengerOnFlight(int id, List<int> FlightsID)
        {
            var passenger = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == id);            
            if (passenger == null)
                return false;

            foreach (var FlightID in FlightsID)
            {
                var flight = await _context.Flights.FindAsync(FlightID);
                if (flight.DepartingTime.AddHours(3).ToUniversalTime() >= DateTime.UtcNow)
                    passenger.Flights.Add(flight);
                else
                    return false;
            }

            _context.Passengers.Update(passenger);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemovePassengerFromFlight(int id, List<int> FlightsID)
        {
            var passenger = await _context.Passengers.Include("Flights").FirstOrDefaultAsync(p => p.ID == id);
            if (passenger == null)
                return false;

            foreach (var FlightID in FlightsID)
            {
                var flight = await _context.Flights.FindAsync(FlightID);
                if (flight.DepartingTime.AddHours(1).ToUniversalTime() <= DateTime.UtcNow)
                    passenger.Flights.Remove(flight);
                else
                    return false;
            }

            _context.Passengers.Update(passenger);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeletePassenger(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger == null)
                return false;

            _context.Passengers.Remove(passenger);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Flight
        public async Task<bool> AddFlight(Flight flight)
        {
            _context.Flights.Add(flight);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        public async Task<List<Flight>?> GetFlights(DateTime? TimeFrom, DateTime? TimeTo)
        {
            if (TimeFrom == null & TimeTo == null)
                return await _context.Flights.ToListAsync();
            else if (TimeFrom == null)
                return await _context.Flights.Where(flight => flight.DepartingTime <= TimeTo).ToListAsync();
            else if (TimeTo == null)
                return await _context.Flights.Where(flight => flight.DepartingTime >= TimeFrom).ToListAsync();
            else
                return await _context.Flights.Where(flight => flight.DepartingTime >= TimeFrom & flight.DepartingTime <= TimeTo).ToListAsync();
        }

        public async Task<Flight?> GetFlight(int id)
        {
            return await _context.Flights.FindAsync(id);
        }

        public async Task<List<Passenger>?> GetPassengerOnFlight(int number)
        {
            var selectedPeople = await _context.Passengers.SelectMany(p => p.Flights,
                            (p, f) => new { Passenger = p, Flight = f })
                          .Where(p => p.Flight.Number == number)
                          .Select(u => u.Passenger).ToListAsync();

            //var test = from passenger in _context.Passengers
            //           from flight in passenger.Flights
            //           where flight.Number == number
            //           select passenger;

            return selectedPeople;
        }

        public async Task<bool> UpdateFlight(Flight flight)
        {
            _context.Flights.Update(flight);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
                return false;

            _context.Flights.Remove(flight);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}

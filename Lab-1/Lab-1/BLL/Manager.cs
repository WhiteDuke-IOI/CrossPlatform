using Lab_1.Data;
using Lab_1.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_1.BLL
{
    public class Manager
    {
        private readonly Lab1Context _context;

        public Manager(Lab1Context context)
        {
            _context = context;
        }

        #region Passenger
        public async Task<bool> PassengerExist(int id)
        {
            if (_context.Passengers == null)
                return false;
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger == null)
                return false;
            return true;
        }

        public async Task<Passenger> AddPassenger(Passenger passenger)
        {
            await _context.Passengers.AddAsync(passenger);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
            return passenger;
        }

        public async Task<List<Passenger>?> GetPassengers()
        {
            //var passengers = await _context.Passengers.Include("Flights").ToListAsync();
            //return passengers.Select(pass => new PassengerDTO(pass)).ToList();
            return _context.Passengers == null ? null : await _context.Passengers.ToListAsync();
        }

        public async Task<Passenger?> GetPassenger(int passengerId)
        {
            return await _context.Passengers.FindAsync(passengerId);
        }

        public async Task<List<Flight>?> GetPassengerFlights(int passengerId, bool current)
        {
            if (_context.Passengers == null)
                return null;
            var passenger = await _context.Passengers.Include(p => p.Flights).FirstOrDefaultAsync(p => p.ID == passengerId);
            if (passenger == null)
                return null;

            if (current)
                return passenger.Flights.Where(flight => flight.DepartingTime.ToUniversalTime() >= DateTime.UtcNow).OrderBy(flight => flight.DepartingTime).ToList();
            else
                return passenger.Flights.OrderBy(flight => flight.DepartingTime).ToList();
        }

        public async Task<bool> UpdatePassenger(Passenger pass)
        {
            //if (_context.Passengers == null)
            //    return false;
            //var passenger = await _context.Passengers.Include(p => p.Flights).FirstOrDefaultAsync(p => p.ID == pass.ID);
            //if (passenger == null)
            //    return false;

            _context.Passengers.Update(pass);

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
            if (_context.Passengers == null)
                return false;
            var passenger = await _context.Passengers.Include(p => p.Flights).FirstOrDefaultAsync(p => p.ID == id);
            if (passenger == null)
                return false;

            foreach (var FlightID in FlightsID)
            {
                var flight = await _context.Flights.FindAsync(FlightID);
                if (flight == null)
                    return false;
                if (flight.DepartingTime.ToUniversalTime() >= DateTime.UtcNow.AddHours(3))
                {
                    if (passenger.Flights.Contains(flight))
                        continue;
                    else
                        passenger.Flights.Add(flight);
                }
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
            if (_context.Passengers == null)
                return false;
            var passenger = await _context.Passengers.Include(p => p.Flights).FirstOrDefaultAsync(p => p.ID == id);
            if (passenger == null)
                return false;

            foreach (var FlightID in FlightsID)
            {
                var flight = await _context.Flights.FindAsync(FlightID);
                if (flight == null)
                    return false;
                if (flight.DepartingTime.ToUniversalTime() >= DateTime.UtcNow.AddHours(1))
                {
                    if (passenger.Flights.Contains(flight))
                        passenger.Flights.Remove(flight);
                    else
                        continue;
                }
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
            if (_context.Passengers == null)
                return false;
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
        public async Task<bool> FlightExist(int id)
        {
            if (_context.Flights == null)
                return false;
            var passenger = await _context.Flights.FindAsync(id);
            if (passenger == null)
                return false;
            return true;
        }

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
            if (_context.Flights == null)
                return null;
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
            if (_context.Flights == null)
                return null;
            return await _context.Flights.FindAsync(id);
        }

        public async Task<List<Passenger>?> GetPassengerOnFlight(int number)
        {
            //var selectedPeople = await _context.Passengers.SelectMany(p => p.Flights,
            //                (p, f) => new { Passenger = p, Flight = f })
            //              .Where(p => p.Flight.Number == number)
            //              .Select(u => u.Passenger).ToListAsync();

            var selectedPeople = from passenger in _context.Passengers
                                 from flight in passenger.Flights
                                 where flight.Number == number
                                 select passenger;

            return await selectedPeople.ToListAsync();
        }

        public async Task<bool> UpdateFlight(Flight flight)
        {
            if (!await FlightExist(flight.Number))
                return false;

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

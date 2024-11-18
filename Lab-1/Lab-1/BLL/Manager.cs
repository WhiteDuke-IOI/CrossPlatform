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

        #region Route
        public async Task<bool> AddRoute(Lab_1.Models.Route route)
        {
            await _context.Routes.AddAsync(route);

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

        public async Task<List<Lab_1.Models.Route>?> GetRoutes()
        {
            return _context.Routes == null ? null : await _context.Routes.ToListAsync();
        }

        public async Task<Lab_1.Models.Route?> GetRoute(string routeName)
        {
            return await _context.Routes.FindAsync(routeName);
        }

        public async Task<bool> UpdateRoute(Lab_1.Models.Route route)
        {
            _context.Routes.Update(route);

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

        public async Task<bool> DeleteRoute(string routeName)
        {
            if (_context.Routes == null)
                return false;
            var route = await _context.Routes.FindAsync(routeName);
            if (route == null)
                return false;

            _context.Routes.Remove(route);

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
            if (_context.Passengers == null | _context.Flights == null)
                return null;
            var passenger = await _context.Passengers.Include(p => p.Flights).ThenInclude(fl => fl.Route).FirstOrDefaultAsync(p => p.ID == passengerId);
            if (passenger == null)
                return null;

            if (current)
                return passenger.Flights.Where(flight => flight.Route.DepartingTime.ToUniversalTime() >= DateTime.UtcNow).OrderBy(flight => flight.Route.DepartingTime).ToList();
            else
                return passenger.Flights.OrderBy(flight => flight.Route.DepartingTime).ToList();
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

        public async Task<int> ModifyPassengerFlight(int id, List<int> FlightsID, bool mode = true)
        {
            if (_context.Passengers == null | _context.Flights == null)
                return 0;
            var passenger = await _context.Passengers.Include(p => p.Flights).FirstOrDefaultAsync(p => p.ID == id);
            if (passenger == null)
                return 0;

            foreach (var FlightID in FlightsID)
            {
                var flight = await _context.Flights.Include(fl => fl.Route).FirstOrDefaultAsync(fl => fl.Number == FlightID);
                if (flight == null)
                    return 0;
                //Console.WriteLine($"Flight: {FlightID}, count = {_context.Passengers.Include(p => p.Flights).Where(fl => fl.Flights.Contains(flight)).ToList().Count()}");
                if (flight.Capacity <= _context.Passengers.Include(p => p.Flights).Where(fl => fl.Flights.Contains(flight)).ToList().Count())
                    return -1;
                if (!passenger.ModifyFlight(flight, mode))
                    return -1;

                _context.Passengers.Update(passenger);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return -1;
            }

            return 1;
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

        public async Task<Flight> AddFlight(FlightDTO flight)
        {
            if (_context.Routes == null)
                return null;
            var route = await _context.Routes.FindAsync(flight.Route);
            var newFlight = new Flight(flight, route);
            _context.Flights.Add(newFlight);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
            return newFlight;
        }

        public async Task<List<Flight>?> GetFlights(DateTime? TimeFrom, DateTime? TimeTo)
        {
            if (_context.Flights == null)
                return null;
            if (TimeFrom == null & TimeTo == null)
                return await _context.Flights.Include(fl => fl.Route).ToListAsync();
            else if (TimeFrom == null)
                return await _context.Flights.Where(flight => flight.Route.DepartingTime <= TimeTo).ToListAsync();
            else if (TimeTo == null)
                return await _context.Flights.Where(flight => flight.Route.DepartingTime >= TimeFrom).ToListAsync();
            else
                return await _context.Flights.Where(flight => flight.Route.DepartingTime >= TimeFrom & flight.Route.DepartingTime <= TimeTo).ToListAsync();
        }

        public async Task<Flight?> GetFlight(int number)
        {
            if (_context.Flights == null)
                return null;
            return await _context.Flights.Include(fl => fl.Route).FirstOrDefaultAsync(fl => fl.Number == number);
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

        public async Task<bool> UpdateFlight(FlightDTO fl)
        {
            if (_context.Flights == null | _context.Routes == null)
                return false;
            var flight = await _context.Flights.Include(f => f.Route).FirstOrDefaultAsync(f => f.Number == fl.Number);
            if (flight == null)
                return false;

            var route = await _context.Routes.FirstOrDefaultAsync(rt => rt.RouteName == fl.Route);
            if (route == null)
                return false;

            flight = new Flight(fl, route);

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

using System.ComponentModel.DataAnnotations;

namespace Lab_1.Models
{
    public class Passenger
    {
        [Key][ScaffoldColumn(false)]
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        //public string Loyalty { get; set; } = string.Empty;
        [Display(Name = "Flight Numbers")]
        public List<Flight>? Flights { get; set; } = null;

        public Passenger() { }

        public Passenger(PassengerDTO pass)
        {
            ID = pass.ID;
            Name = pass.Name;
            Surname = pass.Surname;
            //Loyalty = pass.Loyalty;
        }

        public static implicit operator Passenger(PassengerDTO pass)
        {
            var passenger = new Passenger();
            passenger.ID = pass.ID;
            passenger.Name = pass.Name;
            passenger.Surname = pass.Surname;
            return passenger;
        }

        public bool ModifyFlight(Flight flight, bool mode)
        {
            bool res = true;
            if (mode)
            {
                if (!Flights.Contains(flight))
                {
                    if (flight.Route.DepartingTime.ToUniversalTime() >= DateTime.UtcNow.AddHours(3))
                        Flights.Add(flight);
                    else
                        res = false;
                }
            }
            else 
            {
                if (Flights.Contains(flight))
                {
                    if (flight.Route.DepartingTime.ToUniversalTime() >= DateTime.UtcNow.AddHours(1))
                        Flights.Remove(flight);
                    else
                        res = false;
                }
            }
            return res;
        }

        public Passenger Update(PassengerDTO pass)
        {
            this.Name = pass.Name; 
            this.Surname = pass.Surname;
            //this.Loyalty = pass.Loyalty;
            return this;
        }
    }

    public class PassengerDTO
    {
        [Key][ScaffoldColumn(false)]
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        //public string Loyalty { get; set; } = string.Empty;

        public PassengerDTO() { }

        public PassengerDTO(Passenger pass)
        {
            this.ID = pass.ID;
            this.Name = pass.Name;
            this.Surname = pass.Surname;
            //this.Loyalty = pass.Loyalty;
        }

        public static implicit operator PassengerDTO(Passenger passenger)
        {
            var pass = new PassengerDTO();
            pass.ID = passenger.ID;
            pass.Name = passenger.Name;
            pass.Surname = passenger.Surname;
            return pass;
        }
    }
}

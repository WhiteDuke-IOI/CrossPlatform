using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab_1.Models
{
    public class Flight
    {
        [Key][ScaffoldColumn(false)]
        public int Number { get; set; }
        public string Operator { get; set; }
        public int Capacity { get; set; }
        public Route Route { get; set; }
        public TimeSpan FlightTime { get { return (Route.ArrivalTime - Route.DepartingTime); } }

        public Flight() { }

        public Flight(FlightDTO flight, Route route) 
        {
            Number = flight.Number;
            Operator = flight.Operator;
            Capacity = flight.Capacity;
            Route = route;
        }
    }

    public class FlightDTO
    {
        public int Number { get; set; }
        public string Operator { get; set; }
        public int Capacity { get; set; }
        public string Route { get; set; }

        public FlightDTO() { }

        public static implicit operator FlightDTO(Flight flight)
        {
            var fl = new FlightDTO();
            fl.Number = flight.Number;
            fl.Operator = flight.Operator;
            fl.Capacity = flight.Capacity;
            fl.Route = flight.Route.RouteName;
            return fl;
        }
    }
}

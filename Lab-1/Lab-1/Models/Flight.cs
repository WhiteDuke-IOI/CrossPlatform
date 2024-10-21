using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab_1.Models
{
    public class Flight
    {
        [Key][ScaffoldColumn(false)]
        public int Number { get; set; }
        public string Route { get; set; }
        public string CityFrom { get; set; }
        public string CityTo { get; set; }
        public DateTime DepartingTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public TimeSpan FlightTime { get { return (DepartingTime - ArrivalTime); } }

        public Flight() { }
    }

    //public class FlightDTO
    //{
    //    public int Number { get; set; }
    //    public string Route { get; set; }
    //    public string CityFrom { get; set; }
    //    public string CityTo { get; set; }
    //    public DateTime DepartingTime { get; set; }
    //    public DateTime ArrivalTime { get; set; }
    //    public TimeSpan FlightTime { get { return (DepartingTime - ArrivalTime); } }

    //    public FlightDTO() { }
    //}
}

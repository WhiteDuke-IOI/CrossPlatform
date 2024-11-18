using System.ComponentModel.DataAnnotations;

namespace Lab_1.Models
{
    public class Route
    {
        [Key][ScaffoldColumn(false)]
        public string RouteName { get; set; }
        public string CityFrom { get; set; }
        public string CityTo { get; set; }

        public DateTime DepartingTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}

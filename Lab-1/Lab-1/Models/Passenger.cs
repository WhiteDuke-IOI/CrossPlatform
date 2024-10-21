using Microsoft.EntityFrameworkCore.Update.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab_1.Models
{
    public class Passenger
    {
        [Key][ScaffoldColumn(false)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Loyalty { get; set; }
        [Display(Name = "Flight Numbers")]
        public List<Flight>? Flights { get; set; }

        //public Passenger(PassengerDTO pass) {
        //    ID = pass.ID;
        //    Name = pass.Name;
        //    Surname = pass.Surname;
        //    Loyalty = pass.Loyalty;
        //}

        public Passenger Update(PassengerDTO pass)
        {
            this.Name = pass.Name; 
            this.Surname = pass.Surname;
            this.Loyalty = pass.Loyalty;
            return this;
        }
    }

    public class PassengerDTO
    {
        [Key][ScaffoldColumn(false)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Loyalty { get; set; }

        //public PassengerDTO(Passenger pass)
        //{
        //    ID = pass.ID;
        //    Name = pass.Name;
        //    Surname = pass.Surname;
        //    Loyalty = pass.Loyalty;
        //}
    }
}

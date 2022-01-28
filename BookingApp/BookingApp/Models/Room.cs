using BookingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int Floor { get; set; }
        public int RoomNumber { get; set; }
        public int NumberOfDoubleBeds { get; set; }
        public int NumberOfSingleBeds { get; set; }
        public int NumbefOfPeople{ get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}

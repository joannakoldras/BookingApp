using BookingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Data
{
    public class Reservation
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Room Room { get; set; }

        public int RoomId { get; set; }
        public int ClientId { get; set; }
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public DateTime DatePayment { get; set; }
        public decimal TotalPrice { get; set; }

        public int NumberOfDays { get; set; }
        public bool IsPaid { get; set; }
    }
}

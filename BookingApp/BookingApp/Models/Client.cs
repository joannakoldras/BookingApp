using BookingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string IdentityCardNumber { get; set; }
        public string  PhoneNumber { get; set; }
        public string Number { get; set; }//email

        public virtual ICollection<Reservation> Reservations { get; set; }
       
       
    }
}

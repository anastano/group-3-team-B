using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class Owner : User
    {
        public List<Accommodation> Accommodations;
        public List<AccommodationReservation> Reservations;
         
        public Owner() 
        {
           Accommodations= new List<Accommodation>();
           Reservations = new List<AccommodationReservation>();
        }

        public Owner(User user) 
        {
            Id = user.Id;
            Password = user.Password;
            Username = user.Username;
            Name = user.Name;
            Surname = user.Surname;
            Age = user.Age;

            Accommodations = new List<Accommodation>();
            Reservations = new List<AccommodationReservation>();
        }
    }
}

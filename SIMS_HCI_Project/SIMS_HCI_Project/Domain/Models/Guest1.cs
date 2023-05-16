using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class Guest1 : User
    {
        public List<AccommodationReservation> Reservations;
        //public  bool isSuperGuest;
        public Guest1()
        {
            Reservations = new List<AccommodationReservation>();
        }
        public Guest1(User user)
        {
            Id = user.Id;
            Password = user.Password;
            Username = user.Username;
            Name = user.Name;
            Surname = user.Surname;
            Age = user.Age;
            Role = user.Role;

            Reservations = new List<AccommodationReservation>();
        }
    }
}

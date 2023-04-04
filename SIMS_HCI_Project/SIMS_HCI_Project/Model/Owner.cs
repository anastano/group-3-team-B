using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Serializer;

namespace SIMS_HCI_Project.Model
{
    public class Owner : User
    {
        //inherited Id, Username and Password
        public List<Accommodation> Accommodations { get; set; }
        public List<AccommodationReservation> Reservations { get; set; }

        public Owner() 
        { 
            Accommodations= new List<Accommodation>();
            Reservations= new List<AccommodationReservation>();
        }

        public Owner(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
            Accommodations = new List<Accommodation>();
            Reservations= new List<AccommodationReservation>();
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
        // inherited ToCSV and FromCSV
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class Guest1 : User
    {
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }

        public Guest1()
        {
            Reservations = new ObservableCollection<AccommodationReservation>();
        }

        public Guest1(string id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
            Reservations = new ObservableCollection<AccommodationReservation>();
        }
        public Guest1(Guest1 temp)
        {
            Id = temp.Id;
            Username = temp.Username;
            Password = temp.Password;
            Reservations = new ObservableCollection<AccommodationReservation>(temp.Reservations);
        }
    }
}

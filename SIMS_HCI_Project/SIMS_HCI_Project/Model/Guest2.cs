using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class Guest2 : User
    {
        public ObservableCollection<TourReservation> Reservations { get; set; }

        public Guest2()
        {
            Reservations = new ObservableCollection<TourReservation>();
        }

        public Guest2(string id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
            Reservations = new ObservableCollection<TourReservation>();
        }
    }
}

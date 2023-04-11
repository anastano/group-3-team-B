using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class Guest2 : User
    {
        public ObservableCollection<TourReservation> Reservations { get; set; }
        public ObservableCollection<TourVoucher> Vouchers { get; set; }

        public Guest2()
        {
            Reservations = new ObservableCollection<TourReservation>();
            Vouchers = new ObservableCollection<TourVoucher>();
        }

        /*public Guest2(int id, string username, string password, UserRole role)
        {
            Id = id;
            Username = username;
            Password = password;
            Role = role;
            Reservations = new ObservableCollection<TourReservation>();
            Vouchers= new ObservableCollection<TourVoucher>();
        }*/

        public Guest2(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Password = user.Password;
            //Role = role;
            Name = user.Name;
            Surname = user.Surname;
            Age = user.Age;
            Reservations = new ObservableCollection<TourReservation>();
            Vouchers = new ObservableCollection<TourVoucher>();
        }
    }
}

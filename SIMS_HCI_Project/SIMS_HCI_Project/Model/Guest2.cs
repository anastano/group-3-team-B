using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class Guest2 : User
    {
        //public ObservableCollection<TourReservation> Reservations { get; set; }

        //
        private ObservableCollection<TourReservation> _reservations { get; set; }
        public ObservableCollection<TourReservation> Reservations
        {
            get
            {
                return _reservations;
            }
            set
            {
                if (value != _reservations)
                {
                    _reservations = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //

        public Guest2()
        {
            Reservations = new ObservableCollection<TourReservation>();
        }

        public Guest2(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
            Reservations = new ObservableCollection<TourReservation>();
        }
    }
}

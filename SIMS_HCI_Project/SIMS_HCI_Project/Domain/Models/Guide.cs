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
    public class Guide : User
    {
        public ObservableCollection<TourTime> TodaysTours {get; set; }
        public ObservableCollection<Tour> Tours { get; set; }

        public Guide()
        {
            TodaysTours = new ObservableCollection<TourTime>();
            Tours = new ObservableCollection<Tour>();
        }

        public Guide(User user)
        {
            Id = user.Id;
            Password = user.Password;
            Username = user.Username;
            Name = user.Name;
            Surname = user.Surname;
            Age = user.Age;
            TodaysTours = new ObservableCollection<TourTime>();
            Tours = new ObservableCollection<Tour>();
        }

        public void AddTodaysTourTimes(List<TourTime> tourTimes)
        {
            foreach(TourTime tourTime in tourTimes)
            {
                if(tourTime.DepartureTime.Date == DateTime.Today)
                {
                    TodaysTours.Add(tourTime);
                }
            }
        }
    }
}

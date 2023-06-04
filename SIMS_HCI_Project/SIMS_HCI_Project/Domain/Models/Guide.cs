﻿using System;
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
        public List<Tour> AllTours { get; set; }
        public List<TourTime> TodaysTours { get; set; }

        public Guide(User user)
        {
            Id = user.Id;
            Password = user.Password;
            Username = user.Username;
            Name = user.Name;
            Surname = user.Surname;
            Age = user.Age;
            Role = user.Role;
            AccountActive = user.AccountActive;
        }

        public bool HasTourInProgress()
        {
            return TodaysTours.Any(t => t.IsInProgress);
        }

        public TourTime GetActiveTour()
        {
            if (TodaysTours == null) return null;

            return TodaysTours.Find(t => t.IsInProgress);
        }
    }
}

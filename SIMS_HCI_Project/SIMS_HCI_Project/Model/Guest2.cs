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
        public ObservableCollection<Tour> Tours { get; set; }

        public Guest2()
        {
            Tours = new ObservableCollection<Tour>();
        }

        public Guest2(string id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
            Tours = new ObservableCollection<Tour>();
        }
    }
}

using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class ProfileViewModel
    {
        public NavigationService NavigationService;
        public Guest2 Guest { get; set; }
        public string FullName { get; set; }
        public ProfileViewModel(Guest2 guest, NavigationService navigationService, Frame ProfileFrame)
        {
            Guest = guest;
            NavigationService = navigationService;
            InitCommands();
            FullName = Guest.Name + " " + Guest.Surname;
        }

        private void InitCommands()
        {
            //todo
        }
    }
}

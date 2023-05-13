using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class ProfileNotificationsViewModel
    {
        public Guest2 Guest { get; set; }
        public NavigationService NavigationService { get; set; }
        public ProfileNotificationsViewModel(Guest2 guest, NavigationService navigationService)
        {
            Guest = guest;
            NavigationService = navigationService;
            InitCommands();
        }
        //napravi fju da boja elemenata koji su procitani bude druhacija od onih koje nisu
        private void InitCommands()
        {
            // todo
        }
    }
}

using SIMS_HCI_Project.WPF.Commands.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class TourRequestsViewModel
    {
        #region Commands
        public GuideNavigationCommands NavigationCommands { get; set; }
        #endregion

        public TourRequestsViewModel()
        {
            InitCommands();
        }

        private void InitCommands()
        {
            NavigationCommands = new GuideNavigationCommands();
        }
    }
}

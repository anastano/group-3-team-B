using SIMS_HCI_Project.WPF.Commands.Global;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class MyProfileViewModel
    {
        #region Commands
        public GuideNavigationCommands NavigationCommands { get; set; }
        #endregion

        public MyProfileViewModel()
        {
            InitCommands();
        }

        private void InitCommands()
        {
            NavigationCommands = new GuideNavigationCommands();
        }
    }
}

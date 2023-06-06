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
using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class MyProfileViewModel
    {
        #region Commands
        public GuideNavigationCommands NavigationCommands { get; set; }
        public RelayCommand ResignCommand { get; set; }
        #endregion

        private JobService _jobService;

        public MyProfileViewModel()
        {
            _jobService = new JobService();

            InitCommands();
        }

        private void InitCommands()
        {
            NavigationCommands = new GuideNavigationCommands();
            ResignCommand = new RelayCommand(ExecutedResignCommandCommand, CanExecuteCommand);
        }

        private void ExecutedResignCommandCommand(object obj)
        {
            _jobService.GuideResign((Guide)App.Current.Properties["CurrentUser"]);
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}

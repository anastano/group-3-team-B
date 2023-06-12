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

        public Guide Guide { get; set; }

        private JobService _jobService;

        public MyProfileViewModel()
        {
            _jobService = new JobService();

            Guide = (Guide)App.Current.Properties["CurrentUser"];

            InitCommands();
        }

        private void InitCommands()
        {
            NavigationCommands = new GuideNavigationCommands();
            ResignCommand = new RelayCommand(ExecutedResignCommandCommand, CanExecuteCommand);
        }

        private void ExecutedResignCommandCommand(object obj)
        {
            string messageBoxText = "Resigning will result in the termination of your access to this application, as well as the cancellation of any future tours you have scheduled.";
            string caption = "Resign";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                _jobService.GuideResign((Guide)App.Current.Properties["CurrentUser"]);
                NavigationCommands.SignOut.Execute(null);
            }
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }
    }
}

using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class MessageViewModel
    {
        public string Message { get; set; }
        public string Message2 { get; set; }
        public MessageView MessageView { get; set; }
        public RelayCommand StopDemoCommand { get; set; }

        public MessageViewModel(MessageView messageView, string message, string  message2) 
        {
            Message = message;
            Message2 = message2;
            MessageView = messageView;
        }

        #region Commands
        private async Task StopDemo()
        {
            if (OwnerMainViewModel.Demo)
            {
                OwnerMainViewModel.CTS.Cancel();
                OwnerMainViewModel.Demo = false;

                Window messageDemoOver = new MessageView("The demo mode is over.", "");
                messageDemoOver.Show();
                await Task.Delay(2500);
                messageDemoOver.Close();
            }
        }

        public void Executed_StopDemoCommand(object obj)
        {
            try
            {
                StopDemo();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Error!");
            }
        }

        public bool CanExecute_StopDemoCommand(object obj)
        {
            return true;
        }

        #endregion

        public void InitCommands()
        {
            StopDemoCommand = new RelayCommand(Executed_StopDemoCommand, CanExecute_StopDemoCommand);
        }
    }
}

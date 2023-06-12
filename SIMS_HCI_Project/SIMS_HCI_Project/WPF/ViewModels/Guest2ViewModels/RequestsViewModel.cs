using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class RequestsViewModel : INotifyPropertyChanged
    {
        public NavigationService NavigationService;
        public Guest2 Guest2 { get; set; }
        public RequestsView RequestsView { get; set; }
        public RelayCommand Help { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
            }
        }


        public RequestsViewModel(Guest2 guest2, NavigationService navigationService, RequestsView requestsView, int selectedTabIndex)
        {
            Guest2 = guest2;
            NavigationService = navigationService;
            RequestsView = requestsView;
            SelectedTabIndex = selectedTabIndex;

            InitCommands();
        }

        public void InitCommands()
        {
            //Help
        }
    }
}

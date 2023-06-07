using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views.Help;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class HelpAllViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public Guest2 Guest { get; set; }
        public NavigationService NavigationService { get; set; }
        private Frame _helpFrame;
        public Frame HelpFrame
        {
            get { return _helpFrame; }
            set
            {
                _helpFrame = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> views;
        public ObservableCollection<string> Views
        {
            get { return views; }
            set
            {
                views = value;
                OnPropertyChanged();
            }
        }
        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SelectionChanged { get; set; }

        public HelpAllViewModel(Guest2 guest, NavigationService navigationService, Frame helpFrame)
        {
            Guest = guest;
            NavigationService = navigationService;
            HelpFrame = helpFrame;
            SelectionChanged = new RelayCommand(OnChange, CanExecute);
            this.SelectedIndex = 0;
            LoadViews();
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        private void OnChange(object obj)
        {
            if (SelectedIndex == 0)
            {
                this.HelpFrame.NavigationService.Navigate(new SearchHelp());
            }
            else if (SelectedIndex == 1)
            {
                this.HelpFrame.NavigationService.Navigate(new ReserveHelp());
            }
            else if (SelectedIndex == 2)
            {
                this.HelpFrame.NavigationService.Navigate(new TourRatingHelp());
            }
            else if (SelectedIndex == 3)
            {
                this.HelpFrame.NavigationService.Navigate(new RequestsHelp());
            }


        }

        private void LoadViews()
        {
            Views = new ObservableCollection<string>();
            Views.Add("Search");
            Views.Add("Reserve");
            Views.Add("Tour rating");
            Views.Add("Requests");
        }
    }
}

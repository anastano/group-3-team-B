using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    public class RenovationRecommendationViewModel : INotifyPropertyChanged
    {
        private NavigationService _navigationService;
        public RelayCommand CancelRecommendationCommand { get; set; }
        public RelayCommand RecommendRenovationCommand { get; set; }
        private RenovationRecommendation _recommendation;
        public RenovationRecommendation Recommendation
        {
            get => _recommendation;
            set
            {
                if (value != _recommendation)
                {
                    _recommendation = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RenovationRecommendationViewModel(NavigationService navigationService, RenovationRecommendation recommendation)
        {
            _navigationService = navigationService;
            Recommendation = recommendation;
            InitCommands();
        }
        public void ExecutedRecommendRenovationCommand(object obj)
        {
            _navigationService.ExecuteRecommendation(Recommendation);
            
        }
        public void ExecutedCancelRecommendationCommand(object obj)
        {
            //potencijalno napravili metodu koja canceluje ovo na null
            _navigationService.NavigationStore.Recommendation = null;
            _navigationService.NavigateBack();

        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            CancelRecommendationCommand = new RelayCommand(ExecutedCancelRecommendationCommand, CanExecute);
            RecommendRenovationCommand = new RelayCommand(ExecutedRecommendRenovationCommand, CanExecute);
        }
    }
}

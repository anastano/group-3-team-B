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
        private String _comment;
        public String Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }
        private UrgencyRenovationLevel _urgencyLevel;
        public UrgencyRenovationLevel UrgencyLevel
        {
            get => _urgencyLevel;
            set
            {
                if (value != _urgencyLevel)
                {
                    _urgencyLevel = value;
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
            Comment = recommendation.Comment;
            UrgencyLevel = recommendation.UrgencyLevel;
            InitCommands();
        }
        public void ExecutedRecommendRenovationCommand(object obj)
        {
            _navigationService.ExecuteRecommendation(new RenovationRecommendation(Comment, UrgencyLevel));
            
        }
        public void ExecutedCancelRecommendationCommand(object obj)
        {
            //potencijalno napravili metodu koja canceluje ovo na null
            //ne moze na null sta ako je htjela nesto da promjeni
            //_navigationService.NavigationStore.Recommendation = null;
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

using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
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
        private RenovationRecommendationService _recommendationsService;
        public RelayCommand CancelRecommendationCommand { get; set; }
        public RelayCommand RecommendRenovationCommand { get; set; }
        //public RenovationRecommendation Recommend { get; set; }
        private RenovationRecommendation _recommend;
        public RenovationRecommendation Recommend
        {
            get => _recommend;
            set
            {
                if (value != _recommend)
                {
                    _recommend = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RenovationRecommendationViewModel(/*AccommodationReservation reservation*/)
        {
            _recommendationsService = new RenovationRecommendationService();
            //_ratingService = new RatingGivenByGuestService();
            //Reservation = reservation;
            //InitialProperties();
            Recommend = new RenovationRecommendation();
            InitCommands();
        }
        public void ExecutedRecommentRenovationCommand(object obj)
        {
            //CurrentViewModel = new RenovationRecommendationViewModel();
        }
        public void ExecutedCancelRecommendationCommand(object obj)
        {
            //IsClosed = true;
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            CancelRecommendationCommand = new RelayCommand(ExecutedCancelRecommendationCommand, CanExecute);
            RecommendRenovationCommand = new RelayCommand(ExecutedRecommentRenovationCommand, CanExecute);
        }
    }
}

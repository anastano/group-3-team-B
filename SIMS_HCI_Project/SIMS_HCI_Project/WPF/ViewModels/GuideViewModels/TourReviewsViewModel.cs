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

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class TourReviewsViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public RelayCommand MarkRatingInvalid { get; set; }
        #endregion

        private TourRatingService _tourRatingService;

        private ObservableCollection<TourRating> _tourRatings;
        public ObservableCollection<TourRating> TourRatings
        {
            get { return _tourRatings; }
            set
            {
                _tourRatings = value;
                OnPropertyChanged();
            }
        }
        public TourRating SelectedRating { get; set; }

        public TourTime Tour { get; set; }

        public TourReviewsViewModel(TourTime tour)
        {
            _tourRatingService = new TourRatingService();

            Tour = tour;
            InitCommands();

            LoadRatings();
        }

        private void InitCommands()
        {
            MarkRatingInvalid = new RelayCommand(ExcutedMarkRatingInvalidCommand, CanExecuteCommand);
        }

        public void ExcutedMarkRatingInvalidCommand(object obj)
        {
            _tourRatingService.MarkAsInvalid(SelectedRating);

            LoadRatings();
        }

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }

        private void LoadRatings()
        {
            TourRatings = new ObservableCollection<TourRating>(_tourRatingService.GetAllByTourId(Tour.Id));
        }
    }
}

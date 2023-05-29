using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class SuggestionsForRenovationViewModel : INotifyPropertyChanged
    {
        private readonly RenovationRecommendationService _recommendationService;
        public SuggestionsForRenovationView SuggestionsForRenovationView { get; set; }
        public Owner Owner { get; set; }

        #region OnPropertyChanged
        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {

                    _accommodationName = value;
                    OnPropertyChanged(nameof(AccommodationName));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        public ObservableCollection<RenovationRecommendation> Recommendations { get; set; }
        public RelayCommand CloseSuggestionsForRenovationViewCommand { get; set;  }
        public RelayCommand SearchSuggestionsCommand { get; set; }

        public SuggestionsForRenovationViewModel(SuggestionsForRenovationView suggestionsForRenovationView, Owner owner)
        {
            InitCommands();

            _recommendationService = new RenovationRecommendationService();

            SuggestionsForRenovationView = suggestionsForRenovationView;
            Owner = owner;
            Recommendations = new ObservableCollection<RenovationRecommendation>(_recommendationService.GetByOwnerId(Owner.Id));
        }

        #region Commands

        public void Executed_SearchSuggestionsCommand(object obj)
        {
            List<RenovationRecommendation> searchResult = _recommendationService.OwnerSearch(AccommodationName, Owner.Id);
            Recommendations.Clear();
            foreach (RenovationRecommendation recommendation in searchResult)
            {
                Recommendations.Add(recommendation);
            }
        }

        public bool CanExecute_SearchSuggestionsCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseSuggestionsForRenovationViewCommand(object obj)
        {
            SuggestionsForRenovationView.Close();
        }

        public bool CanExecute_CloseSuggestionsForRenovationViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            SearchSuggestionsCommand = new RelayCommand(Executed_SearchSuggestionsCommand, CanExecute_SearchSuggestionsCommand);
            CloseSuggestionsForRenovationViewCommand = new RelayCommand(Executed_CloseSuggestionsForRenovationViewCommand, CanExecute_CloseSuggestionsForRenovationViewCommand);
        }
    }
}

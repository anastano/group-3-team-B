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
    public class RenovationSuggestionsViewModel : INotifyPropertyChanged
    {
        private readonly RenovationRecommendationService _recommendationService;
        public RenovationSuggestionsView RenovationSuggestionsView { get; set; }
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
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set;  }

        public RenovationSuggestionsViewModel(RenovationSuggestionsView renovationSuggestionsView, Owner owner)
        {
            InitCommands();

            _recommendationService = new RenovationRecommendationService();

            RenovationSuggestionsView = renovationSuggestionsView;
            Owner = owner;
            Recommendations = new ObservableCollection<RenovationRecommendation>(_recommendationService.GetByOwnerId(Owner.Id));
        }

        #region Commands

        public void Executed_SearchCommand(object obj)
        {
            List<RenovationRecommendation> searchResult = _recommendationService.OwnerSearch(AccommodationName, Owner.Id);
            Recommendations.Clear();
            foreach (RenovationRecommendation recommendation in searchResult)
            {
                Recommendations.Add(recommendation);
            }
        }

        public void Executed_CloseViewCommand(object obj)
        {
            RenovationSuggestionsView.Close();
        }

        #endregion

        public void InitCommands()
        {
            SearchCommand = new RelayCommand(Executed_SearchCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
        }
    }
}

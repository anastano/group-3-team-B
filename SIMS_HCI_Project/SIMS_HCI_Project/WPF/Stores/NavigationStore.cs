using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.Stores
{
    public class NavigationStore
    {
        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }
        private object _previousViewModel;
        public object PreviousViewModel
        {
            get => _previousViewModel;
            set
            {
                _previousViewModel = value;
                OnCurrentViewModelChanged();
            }
        }
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnCurrentViewModelChanged();

            }
        }
        private string _previousTitle;
        public string PreviousTitle
        {
            get => _previousTitle;
            set
            {
                _previousTitle = value;
                OnCurrentViewModelChanged();

            }
        }
        private RenovationRecommendation _recommendation;
        public RenovationRecommendation Recommendation
        {
            get => _recommendation;
            set
            {
                _recommendation = value;
                //OnCurrentViewModelChanged();

            }
        }
        public event Action CurrentViewModelChanged;
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}

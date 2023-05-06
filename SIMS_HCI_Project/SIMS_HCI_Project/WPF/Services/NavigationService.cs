using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Stores;
using SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization;

namespace SIMS_HCI_Project.WPF.Services
{
    public class NavigationService
    {
        public NavigationStore _navigationStore;
        public event Action CurrentViewModelChanged;
        public NavigationService()
        {
            _navigationStore = new NavigationStore();
        }
        public void Navigate(object viewModel, string title)
        {
            _navigationStore.PreviousTitle = _navigationStore.Title;
            _navigationStore.Title = title;
            _navigationStore.PreviousViewModel = _navigationStore.CurrentViewModel;
            _navigationStore.CurrentViewModel = viewModel;
            OnCurrentViewModelChanged();
        }
        public void NavigateBack()
        {
            _navigationStore.Title = _navigationStore.PreviousTitle;
            object temp = _navigationStore.PreviousViewModel;
            _navigationStore.PreviousViewModel = _navigationStore.CurrentViewModel;
            _navigationStore.CurrentViewModel = temp;
            OnCurrentViewModelChanged();
        }
        public void ExecuteRecommendation(RenovationRecommendation recommendation)
        {
            _navigationStore.Recommendation = recommendation;
            NavigateBack();
            OnCurrentViewModelChanged();
        }
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}

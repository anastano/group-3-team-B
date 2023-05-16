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
        public NavigationStore NavigationStore;
        public event Action CurrentViewModelChanged;
        public event Action RecommendationChanged;
        public NavigationService()
        {
            NavigationStore = new NavigationStore();
        }
        public void Navigate(object viewModel, string title)
        {
            NavigationStore.PreviousTitle = NavigationStore.Title;
            NavigationStore.Title = title;
            NavigationStore.PreviousViewModel = NavigationStore.CurrentViewModel;
            NavigationStore.CurrentViewModel = viewModel;
            NavigationStore.Recommendation = null;
            OnCurrentViewModelChanged();
        }
        public void NavigateBack()
        {
            NavigationStore.Title = NavigationStore.PreviousTitle;
            object temp = NavigationStore.PreviousViewModel;
            NavigationStore.PreviousViewModel = NavigationStore.CurrentViewModel;
            NavigationStore.CurrentViewModel = temp;
            OnCurrentViewModelChanged();
        }
        public void ExecuteRecommendation(RenovationRecommendation recommendation)
        {
            NavigationStore.Recommendation = recommendation;
            NavigateBack();
            OnCurrentViewModelChanged();
            OnRecommendationChanged();
        }
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
        private void OnRecommendationChanged()
        {
            RecommendationChanged?.Invoke();
        }
    }
}

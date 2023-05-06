using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class AllToursStatisticsViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private TourStatisticsService _tourStatisticsService;
        private TourService _tourService;

        public TourTime AllTimeTopTour { get; set; }

        private TourTime _selectedYearTopTour;
        public TourTime SelectedYearTopTour
        {
            get { return _selectedYearTopTour; }
            set
            {
                _selectedYearTopTour = value;
                OnPropertyChanged();
            }
        }

        public List<int> YearsWithTours { get; set; }

        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                UpdateTopTourByYear();
            }
        }

        public GuideNavigationCommands NavigationCommands { get; set; }

        public AllToursStatisticsViewModel(Guide guide)
        {
            _tourStatisticsService = new TourStatisticsService();
            _tourService = new TourService();

            AllTimeTopTour = _tourStatisticsService.GetTopTour();
            YearsWithTours = _tourService.GetAllTourInstances().Where(tt => tt.Status == TourStatus.COMPLETED)
                                                                .Select(tt => tt.DepartureTime.Year).Distinct().ToList();
            SelectedYear = YearsWithTours.First();

            UpdateTopTourByYear();
            InitCommands();
        }

        private void UpdateTopTourByYear()
        {
            SelectedYearTopTour = _tourStatisticsService.GetTopTourByYear(SelectedYear);
        }

        private void InitCommands()
        {
            NavigationCommands = new GuideNavigationCommands();
        }
    }
}

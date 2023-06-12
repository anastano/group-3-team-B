using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
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

        public GuideNavigationCommands NavigationCommands { get; set; }

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

        private TourStatisticsInfo _allTimeTopTourStatistics;
        public TourStatisticsInfo AllTimeTopTourStatistics
        {
            get { return _allTimeTopTourStatistics; }
            set
            {
                _allTimeTopTourStatistics = value;
                OnPropertyChanged();
            }
        }

        private TourStatisticsInfo _selectedYearTopTourStatistics;
        public TourStatisticsInfo SelectedYearTopTourStatistics
        {
            get { return _selectedYearTopTourStatistics; }
            set
            {
                _selectedYearTopTourStatistics = value;
                OnPropertyChanged();
            }
        }

        public String AllTimeTopTourImage { get; set; }

        public String _selectedYearTopTourImage;
        public String SelectedYearTopTourImage
        {
            get { return _selectedYearTopTourImage; }
            set
            {
                _selectedYearTopTourImage = value;
                OnPropertyChanged();
            }
        }

        public AllToursStatisticsViewModel()
        {
            _tourStatisticsService = new TourStatisticsService();
            _tourService = new TourService();

            AllTimeTopTour = _tourStatisticsService.GetTopTour();
            YearsWithTours = _tourService.GetAllTourInstances().Where(tt => tt.Status == TourStatus.COMPLETED)
                                                                .Select(tt => tt.DepartureTime.Year).Distinct().ToList();
            if (YearsWithTours != null)
            {
                SelectedYear = YearsWithTours.First();
            }

            AllTimeTopTourStatistics = _tourStatisticsService.GetTourStatistics(AllTimeTopTour.Id);
            SelectedYearTopTourStatistics = _tourStatisticsService.GetTourStatistics(SelectedYearTopTour.Id);

            AllTimeTopTourImage = AllTimeTopTour.Tour.Images.First();

            UpdateTopTourByYear();
            InitCommands();
        }

        private void UpdateTopTourByYear()
        {
            SelectedYearTopTour = _tourStatisticsService.GetTopTourByYear(SelectedYear);
            SelectedYearTopTourStatistics = _tourStatisticsService.GetTourStatistics(SelectedYearTopTour.Id);
            SelectedYearTopTourImage = SelectedYearTopTour.Tour.Images.First();
        }

        private void InitCommands()
        {
            NavigationCommands = new GuideNavigationCommands();
        }
    }
}

using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
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
        private TourTimeService _tourTimeService;

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

        public AllToursStatisticsViewModel(Guide guide)
        {
            _tourStatisticsService = new TourStatisticsService();
            _tourTimeService = new TourTimeService();

            AllTimeTopTour = _tourStatisticsService.GetTopTour();
            YearsWithTours = _tourTimeService.GetYearsWithToursByGuide(guide.Id);
            SelectedYear = YearsWithTours.First();
            UpdateTopTourByYear();
        }

        private void UpdateTopTourByYear()
        {
            SelectedYearTopTour = _tourStatisticsService.GetTopTourByYear(SelectedYear);
        }
    }
}

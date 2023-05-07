using LiveCharts;
using LiveCharts.Wpf;
using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Navigation;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.WPF.Commands;
using Axis = LiveCharts.Wpf.Axis;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        public Guest2 Guest2 { get; set; }  
        public NavigationService NavigationService { get; set; }
        private RegularTourRequestService _regularTourRequestService { get; set; }

        private TourRequestsStatisticsService _tourRequestsStatisticsService;
        private TourRequestsStatisticsByStatus _tourRequestsStatisticsByStatus;
        public TourRequestsStatisticsByStatus TourRequestsStatisticsByStatus
        {
            get { return _tourRequestsStatisticsByStatus; }
            set 
            {
                if (value != _tourRequestsStatisticsByStatus)
                {
                    _tourRequestsStatisticsByStatus = value;
                    OnPropertyChanged();
                }
            }
        }

      
        private Dictionary<string, int> _requestsByLanguage;

        public Dictionary<string, int> RequestsByLanguage
        {
            get => _requestsByLanguage;
            set
            {
                if (value != _requestsByLanguage)
                {
                    _requestsByLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        public ChartValues<string> RequestsByLanguageKeys
        {
            get => new ChartValues<string>(_requestsByLanguage.Keys);
        }

        private ChartValues<int> _requestsByLanguageValues;

        public ChartValues<int> RequestsByLanguageValues
        {
            get => _requestsByLanguageValues;
            set
            {
                if (value != _requestsByLanguageValues)
                {
                    _requestsByLanguageValues = value;
                    OnPropertyChanged();
                }
            }
        }


        private ChartValues<int> _acceptedCount;
        public ChartValues<int> AcceptedCount

        {
            get => new ChartValues<int> { TourRequestsStatisticsByStatus.RequestsNumberByStatus[RegularRequestStatus.ACCEPTED] };
            set
            {
                if (value != _acceptedCount)
                {
                    _acceptedCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<int> _invalidCount;
        public ChartValues<int> InvalidCount

        {
            get => new ChartValues<int> { TourRequestsStatisticsByStatus.RequestsNumberByStatus[RegularRequestStatus.INVALID] };
            set
            {
                if (value != _invalidCount)
                {
                    _invalidCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private ChartValues<int> _pendingCount;
        public ChartValues<int> PendingCount

        {
            get => new ChartValues<int> { TourRequestsStatisticsByStatus.RequestsNumberByStatus[RegularRequestStatus.PENDING] };
            set
            {
                if (value != _pendingCount)
                {
                    _pendingCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedYearIndex; 
        public int SelectedYearIndex
        {
            get => _selectedYearIndex;
            set
            {
                _selectedYearIndex = value;
                OnPropertyChanged(nameof(SelectedYearIndex));
                ExecuteSelectedYearChanged();
            }
        }


        private List<RegularTourRequest> _requests;
        public List<RegularTourRequest> Requests
        {
            get { return _requests; }
            set
            {
                _requests = value;
                OnPropertyChanged();
            }
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public Func<ChartPoint, string> ColumnChartLabelPoint =>
              chartPoint =>
              {
                  string language = RequestsByLanguageKeys[(int)chartPoint.X];
                  var count = chartPoint.Y;
                  return $"{language}: {count}";
              };


        public StatisticsViewModel(Guest2 guest, NavigationService navigationService)
        {
            Guest2 = guest;
            NavigationService = navigationService;

            LoadFromFiles();

            SelectedYearIndex = 0;

            TourRequestsStatisticsByStatus = _tourRequestsStatisticsService.GetTourRequestsStatisticsByStatus(Guest2.Id);


            Requests = new List<RegularTourRequest>(_regularTourRequestService.GetAllByGuestId(Guest2.Id));


            RequestsByLanguage = new Dictionary<string, int>(_tourRequestsStatisticsService.GetTourRequestStatisticsByLanguages(Guest2.Id));

            RequestsByLanguageValues = new ChartValues<int>(RequestsByLanguage.Values);

        
        }

        private void ExecuteSelectedYearChanged()
        {
            if (SelectedYearIndex == 0)
            {
                TourRequestsStatisticsByStatus = _tourRequestsStatisticsService.GetTourRequestsStatisticsByStatus(Guest2.Id);
                UpdateChart(TourRequestsStatisticsByStatus); //needed here?
            }
            else
            {
                int selectedYear = SelectedYearIndex+2020;
                TourRequestsStatisticsByStatus = _tourRequestsStatisticsService.GetTourRequestsStatisticsByStatus(Guest2.Id, selectedYear);
                UpdateChart(TourRequestsStatisticsByStatus);
            }
        }

        private void UpdateChart(TourRequestsStatisticsByStatus TourRequestsStatisticsByStatus)
        {
            InvalidCount = new ChartValues<int> { TourRequestsStatisticsByStatus.RequestsNumberByStatus[RegularRequestStatus.INVALID] };
            AcceptedCount = new ChartValues<int> { TourRequestsStatisticsByStatus.RequestsNumberByStatus[RegularRequestStatus.ACCEPTED] };
            PendingCount = new ChartValues<int> { TourRequestsStatisticsByStatus.RequestsNumberByStatus[RegularRequestStatus.PENDING] };
        }


        private void LoadFromFiles()
        {
            _regularTourRequestService = new RegularTourRequestService();
            _tourRequestsStatisticsService = new TourRequestsStatisticsService();
        }

        
    }
}

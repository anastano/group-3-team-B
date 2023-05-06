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


namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        public Guest2 Guest2 { get; set; }  
        public NavigationService NavigationService { get; set; }
        private RegularTourRequestService _regularTourRequestService { get; set; }

        private TourRequestsStatisticsService _tourRequestsStatisticsService;
        public TourRequestsStatisticsByStatus TourRequestsStatisticsByStatus { get; set; }

        public int CountAccepted { get; set; }
        public int CountPending { get; set; }
        public int CountInvalid { get; set; }

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

        private int? _selectedYear;
        public int? SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
                //UpdateStatistics();
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
        private ObservableCollection<LiveCharts.Wpf.PieSeries> _requestStatusSummary;

        public ObservableCollection<LiveCharts.Wpf.PieSeries> RequestStatusSummary
        {
            get { return _requestStatusSummary; }
            set
            {
                _requestStatusSummary = value;
                OnPropertyChanged(nameof(RequestStatusSummary));
            }
        }

        public IEnumerable<LiveCharts.Wpf.PieSeries> RequestStatusSummaryAAA
        {
            get
            {
                var groupedList = Requests
                    .GroupBy(r => r.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() });

                var seriesCollection = new SeriesCollection();
                foreach (var group in groupedList)
                {
                    var series = new LiveCharts.Wpf.PieSeries
                    {
                        Title = group.Status.ToString(),
                        Values = new ChartValues<double> { group.Count }
                    };
                    seriesCollection.Add(series);
                }

                return seriesCollection as IEnumerable<LiveCharts.Wpf.PieSeries> ;
            }
        } 


        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public StatisticsViewModel(Guest2 guest, NavigationService navigationService)
        {
            Guest2 = guest;
            NavigationService = navigationService;

            LoadFromFiles();
            InitCommands();

            TourRequestsStatisticsByStatus = _tourRequestsStatisticsService.GetTourRequestsStatisticsByStatus(Guest2.Id);

            CountAccepted = TourRequestsStatisticsByStatus.RequestsNumberByStatus[RegularRequestStatus.ACCEPTED];
            CountPending = TourRequestsStatisticsByStatus.RequestsNumberByStatus[RegularRequestStatus.PENDING];
            CountInvalid = TourRequestsStatisticsByStatus.RequestsNumberByStatus[RegularRequestStatus.INVALID];


        //TourRequestsStatisticsByStatus.RequestsNumberByStatus();

        _requestStatusSummary = new ObservableCollection<LiveCharts.Wpf.PieSeries>
        {
            new LiveCharts.Wpf.PieSeries
            {
                Title = "Pending",
                Values = new ChartValues<int> { 10 }
            },
            new LiveCharts.Wpf.PieSeries
            {
                Title = "Accepted",
                Values = new ChartValues<int> { 20 }
            },
            new LiveCharts.Wpf.PieSeries
            {
                Title = "Invalid",
                Values = new ChartValues<int> { 5 }
            }
        };

            Requests = new List<RegularTourRequest>(_regularTourRequestService.GetAllByGuestId(Guest2.Id));
        }

        public void GetFieldsByStatus(int guestId, RegularRequestStatus status)
        {
            
        }

        private void InitCommands()
        {
            _regularTourRequestService = new RegularTourRequestService();
            _tourRequestsStatisticsService = new TourRequestsStatisticsService();
        }

        private void LoadFromFiles()
        {

        }
    }
}

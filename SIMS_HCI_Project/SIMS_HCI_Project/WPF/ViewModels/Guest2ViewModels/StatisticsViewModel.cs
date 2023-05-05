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

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        public Guest2 Guest2 { get; set; }  
        public NavigationService NavigationService { get; set; }
        private RegularTourRequestService _regularTourRequestService { get; set; }

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



        /*public IEnumerable<PieChart> StatusSummary
        {
            get
            {
                var summary = Requests
                    .GroupBy(request => request.Status)
                    .Select(group => new LiveCharts.Wpf.PieSeries
                    {
                        Title = group.Key.ToString(), 
                        Values = new ChartValues<double> { group.Count() }
                    });

                return summary;
            }
        }*/
        public IEnumerable<LiveCharts.Wpf.PieSeries> RequestStatusSummary
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
            Requests = new List<RegularTourRequest>(_regularTourRequestService.GetAllByGuestId(Guest2.Id));
        }

        private void InitCommands()
        {
            _regularTourRequestService = new RegularTourRequestService();
        }

        private void LoadFromFiles()
        {

        }
    }
}

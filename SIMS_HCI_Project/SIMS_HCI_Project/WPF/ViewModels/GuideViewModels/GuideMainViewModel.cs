using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class GuideMainViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public TourTime TourInProgress { get; set; }
        public ObservableCollection<TourTime> TodaysTours { get; set; }

        public Guide Guide { get; set; }

        private TourService _tourService;

        public GuideMainViewModel(Guide guide)
        {
            Guide = guide;

            _tourService = new TourService();
        }

        private void LoadTourInProgress()
        {

        }

        private void LoadTodaysTours()
        {

        }

    }
}

using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class GuideMainViewModel
    {
        private TourTimeService _tourTimeService;
        private TourVoucherService _tourVoucherService;
        private TourReservationService _tourReservationService;
        private TourService _tourService;

        public ObservableCollection<TourTime> AllTourTimes { get; set; }
        public TourTime SelectedTourTime { get; set; }

        public RelayCommand CancelTourCommand { get; set; }

        public GuideMainViewModel(Guide guide)
        {
            LoadFromFiles();
            CancelTourCommand = new RelayCommand(Excuted_CancelTourCommand, CanExecute_CancelTourCommand);

            AllTourTimes = new ObservableCollection<TourTime>(_tourTimeService.GetAllByGuideId(guide.Id));
            SelectedTourTime = AllTourTimes.First();
        }

        private void LoadFromFiles()
        {
            _tourTimeService = new TourTimeService();
            _tourVoucherService = new TourVoucherService();
            _tourReservationService = new TourReservationService();
            _tourService = new TourService();

            _tourService.ConnectDepartureTimes(_tourTimeService);
        }


        public void Excuted_CancelTourCommand(object obj)
        {
            _tourTimeService.CancelTour(SelectedTourTime, _tourVoucherService, _tourReservationService);
        }

        public bool CanExecute_CancelTourCommand(object obj)
        {
            return true;
        }
    }
}

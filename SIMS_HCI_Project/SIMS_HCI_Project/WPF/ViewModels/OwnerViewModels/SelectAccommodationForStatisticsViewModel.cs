using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class SelectAccommodationForStatisticsViewModel
    {
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _reservationService;
        public SelectAccommodationForStatisticsView SelectAccommodationForStatisticsView { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public RelayCommand SelectAccommodationForStatisticsCommand { get; set; }
        public RelayCommand CloseSelectAccommodationForStatisticsViewCommand { get; set; }

        public SelectAccommodationForStatisticsViewModel(SelectAccommodationForStatisticsView selectAccommodationForStatisticsView, Owner owner)
        {
            InitCommands();

            _accommodationService = new AccommodationService();
            _reservationService = new AccommodationReservationService();

            SelectAccommodationForStatisticsView = selectAccommodationForStatisticsView;
            Owner = owner;
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetByOwnerId(Owner.Id));
        }

        #region Commands
        public void Executed_SelectAccommodationForStatisticsCommand(object obj)
        {
            if (SelectedAccommodation != null)
            {
                if (_reservationService.GetByAccommodationId(SelectedAccommodation.Id).Count() != 0)
                {
                    Window statisticsByYearView = new StatisticsByYearView(SelectAccommodationForStatisticsView, SelectedAccommodation);
                    statisticsByYearView.ShowDialog();
                }
                else 
                {
                    MessageBox.Show("Currently, there are no available statistics for this accommodation as there have been no reservations made yet.");
                }
            }
            else
            {
                MessageBox.Show("No accommodation has been selected.");
            }
        }

        public bool CanExecute_SelectAccommodationForStatisticsCommand(object obj)
        {
            return true;
        }


        public void Executed_CloseSelectAccommodationForStatisticsViewCommand(object obj)
        {
            SelectAccommodationForStatisticsView.Close();
        }

        public bool CanExecute_CloseSelectAccommodationForStatisticsViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            SelectAccommodationForStatisticsCommand = new RelayCommand(Executed_SelectAccommodationForStatisticsCommand, CanExecute_SelectAccommodationForStatisticsCommand);
            CloseSelectAccommodationForStatisticsViewCommand = new RelayCommand(Executed_CloseSelectAccommodationForStatisticsViewCommand, CanExecute_CloseSelectAccommodationForStatisticsViewCommand);
        }
    }
}

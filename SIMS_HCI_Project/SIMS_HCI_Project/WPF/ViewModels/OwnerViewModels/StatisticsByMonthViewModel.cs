using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class StatisticsByMonthViewModel
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly AccommodationStatisticsService _statisticsService;

        public StatisticsByMonthView StatisticsByMonthView { get; set; }
        public StatisticsByYearView StatisticsByYearView { get; set; }
        public Accommodation Accommodation { get; set; }
        public AccommodationYear AccommodationYear { get; set; }
        public string BestMonth { get; set; }
        public List<AccommodationMonth> AccommodationMonths { get; set; }

        public StatisticsByMonthViewModel(StatisticsByMonthView statisticsByMonthView, StatisticsByYearView statisticsByYearView,
            AccommodationReservationService reservationService, AccommodationStatisticsService statisticsService,
            Accommodation selectedAccommodation, AccommodationYear accommodationYear)
        {
            _reservationService = reservationService;
            _statisticsService = statisticsService;

            StatisticsByMonthView = statisticsByMonthView;
            StatisticsByYearView = statisticsByYearView;

            Accommodation = selectedAccommodation;
            AccommodationYear = accommodationYear;
            BestMonth = _statisticsService.FindBestMonthInYear(AccommodationYear.Year, Accommodation.Id);
            AccommodationMonths = _statisticsService.GetMonthsByAccommodationIdAndYear(Accommodation.Id, AccommodationYear.Year);
        }
    }
}

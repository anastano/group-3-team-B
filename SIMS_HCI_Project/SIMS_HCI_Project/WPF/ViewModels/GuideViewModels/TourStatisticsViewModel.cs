using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class TourStatisticsViewModel
    {
        private TourStatisticsService _tourStatisticsService;

        public TourStatisticsInfo TourStatistics { get; set; }

        public TourStatisticsViewModel(TourTime tour)
        {
            _tourStatisticsService = new TourStatisticsService();

            TourStatistics = _tourStatisticsService.GetTourStatistics(tour.Id);
        }


    }
}

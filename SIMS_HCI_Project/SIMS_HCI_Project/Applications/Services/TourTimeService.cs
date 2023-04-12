﻿using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourTimeService
    {
        private readonly ITourTimeRepository _tourTimeRepository;

        public TourTimeService()
        {
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
        }

        public void Load()
        {
            _tourTimeRepository.Load();
        }

        public void Save()
        {
            _tourTimeRepository.Save();
        }

        public TourTime GetById(int id)
        {
            return _tourTimeRepository.GetById(id);
        }

        public List<TourTime> GetAll()
        {
            return _tourTimeRepository.GetAll();
        }

        public List<TourTime> GetAllByGuideId(int id)
        {
            return _tourTimeRepository.GetAllByGuideId(id);
        }

        public void CancelTour(TourTime tourTime, TourVoucherService tourVoucherService, TourReservationService tourReservationService)
        {
            _tourTimeRepository.CancelTour(tourTime);
            List<TourReservation> cancelledReservations = tourReservationService.CancelReservationsByTour(tourTime.TourId);
            tourVoucherService.GiveVouchersToGuestsWithReservation(cancelledReservations);
        }
    }
}
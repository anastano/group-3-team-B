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

       /* public TourTime GetByReservationId(int id)
        {
            return _tourTimeRepository.GetByReservationId(id);
        }
       */
        /*public TourTime GetByReservationId(int id, TourReservationService tourReservationService)
        {
            return _tourTimeRepository.GetByReservationId(id, tourReservationService);
            foreach (TourReservation tourReservation in tourReservationService.GetAll())
            {
                return _tourTimes.Find(tt => tt.Id == id);
            }
        }*/

        public void CancelTour(TourTime tourTime, TourVoucherService tourVoucherService, TourReservationService tourReservationService)
        {
            _tourTimeRepository.CancelTour(tourTime);
            List<TourReservation> cancelledReservations = tourReservationService.CancelReservationsByTour(tourTime.TourId);
            tourVoucherService.GiveVouchersToGuestsWithReservation(cancelledReservations);
        }
        public void ConnectGuestAttendances(GuestTourAttendanceService guestTourAttendanceService)
        {
            foreach (TourTime tourTime in _tourTimeRepository.GetAll())
            {
                tourTime.GuestAttendances = guestTourAttendanceService.GetAllByTourId(tourTime.Id);
            }
        }
        public void ConnectCurrentKeyPoints()
        {
            foreach (TourTime tourTime in _tourTimeRepository.GetAll())
            {
                tourTime.CurrentKeyPoint = tourTime.Tour.KeyPoints[tourTime.CurrentKeyPointIndex];
            }
        }
        public void ReduceAvailablePlaces(TourTime selectedTourTime, int requestedPartySize)
        {
            TourTime tourTime = GetById(selectedTourTime.Id);

            tourTime.Available -= requestedPartySize;
        }
        public void CheckAndUpdateStatus()
        {
            _tourTimeRepository.CheckAndUpdateStatus();
        }
    }
}

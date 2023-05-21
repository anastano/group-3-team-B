﻿using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class TourReservationRepository : ITourReservationRepository
    {
        private readonly TourReservationFileHandler _fileHandler;
        private static List<TourReservation> _reservations;

        public TourReservationRepository()
        {
            _fileHandler = new TourReservationFileHandler();
            if (_reservations == null)
            {
                Load();
            }
        }

        public void Load()
        {
            _reservations = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_reservations);
        }

        public TourReservation GetById(int id)
        {
            return _reservations.Find(r => r.Id == id);
        }

        public List<TourReservation> GetAll()
        {
            return _reservations;
        }

        public List<TourReservation> GetAllByGuestId(int id)
        {
            return _reservations.FindAll(r => r.Guest2Id == id);
        }

        public List<TourReservation> GetAllByTourTimeId(int id)
        {
            return _reservations.FindAll(r => r.TourTimeId == id);
        }

        public List<TourReservation> GetAllByGuestIdAndTourId(int guestId, int tourId)
        {
            return _reservations.FindAll(r => r.Guest2Id == guestId && r.TourTime.Tour.Id == tourId);
        }

        public TourReservation GetByGuestAndTour(int guestId, int tourTimeId)
        {
            return _reservations.Where(tr => tr.Guest2Id == guestId && tr.TourTimeId == tourTimeId).First();
        }
       
        public void BulkUpdate(List<TourReservation> tourReservations)
        {
            foreach (TourReservation tourReservation in tourReservations)
            {
                TourReservation toUpdate = GetById(tourReservation.Id);
                toUpdate = tourReservation;
            }
            Save();
        }

        public List<TourReservation> GetActiveByGuestId(int id)
        {
            return _reservations.FindAll(r => r.Guest2Id == id && r.TourTime.Status == TourStatus.IN_PROGRESS && r.Status == TourReservationStatus.GOING);
        }

        private int GenerateId()
        {
            return _reservations.Count == 0 ? 1 : _reservations[_reservations.Count - 1].Id + 1;
        }

        public void Add(TourReservation tourReservation)
        {
            tourReservation.Id = GenerateId();
            _reservations.Add(tourReservation);
            Save();
        }
    }
}

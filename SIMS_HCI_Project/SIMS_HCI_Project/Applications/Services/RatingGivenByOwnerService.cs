﻿using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class RatingGivenByOwnerService
    {
        private readonly IRatingGivenByOwnerRepository _ratingRepository;

        public RatingGivenByOwnerService()
        {
            _ratingRepository = Injector.Injector.CreateInstance<IRatingGivenByOwnerRepository>();
        }
        public void Save()
        {
            _ratingRepository.Save();
        }
        public RatingGivenByOwner GetById(int id)
        {
            return _ratingRepository.GetById(id);
        }

        public RatingGivenByOwner GetByReservationId(int reservationId)
        {
            return _ratingRepository.GetByReservationId(reservationId);
        }

        public List<RatingGivenByOwner> GetAll()
        {
            return _ratingRepository.GetAll();
        }

        public void Add(RatingGivenByOwner rating)
        {
            _ratingRepository.Add(rating);
        }

        public void ConnectRatingsWithReservations(AccommodationReservationService reservationService)
        {
            foreach (RatingGivenByOwner rating in _ratingRepository.GetAll())
            {
                rating.Reservation = reservationService.GetById(rating.ReservationId);
            }
        }

        public void NotifyObservers()
        {
            _ratingRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _ratingRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _ratingRepository.Unsubscribe(observer);
        }
    }
}

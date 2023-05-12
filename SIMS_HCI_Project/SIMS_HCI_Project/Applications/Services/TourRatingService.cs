﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourRatingService
    {
        private readonly ITourRatingRepository _tourRatingRepository;

        public TourRatingService()
        {
            _tourRatingRepository = Injector.Injector.CreateInstance<ITourRatingRepository>();
        }

        public List<TourRating> GetAllByTourId(int tourTimeId)
        {
            return _tourRatingRepository.GetAllByTourId(tourTimeId);
        }

        public bool IsRated(int reservationId)
        {
            return _tourRatingRepository.IsRated(reservationId);
        }

        public void Add(TourRating tourRating)
        {
            _tourRatingRepository.Add(tourRating);
        }

        public void MarkAsInvalid(TourRating tourRating)
        {
            tourRating.IsValid = false;
            _tourRatingRepository.Update(tourRating);
        }
    }
}

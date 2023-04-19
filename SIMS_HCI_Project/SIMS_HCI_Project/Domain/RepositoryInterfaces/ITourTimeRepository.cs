﻿using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourTimeRepository
    {
        TourTime GetById(int id);
        List<TourTime> GetAll();
        List<TourTime> GetAllByGuideId(int guideId);

        void Add(TourTime tourTime);
        void AddMultiple(List<TourTime> tourTimes);
        void Update(TourTime tourTime);

        bool HasTourInProgress(int guideId);

        void CheckAndUpdateStatus();
        void AssignTourToTourTimes(Tour tour, List<TourTime> tourTimes);
    }
}
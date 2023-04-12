﻿using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IRatingGivenByOwnerRepository
    {
        void Add(RatingGivenByOwner rating);
        int GenerateId();
        List<RatingGivenByOwner> GetAll();
        RatingGivenByOwner GetById(int id);
        RatingGivenByOwner GetByReservationId(int reservationId);
        void NotifyObservers();
        void Save();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}
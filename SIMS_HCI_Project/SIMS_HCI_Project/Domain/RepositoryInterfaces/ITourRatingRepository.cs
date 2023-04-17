using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourRatingRepository
    {
        void Load();
        void Save();
        List<TourRating> GetAll();
        TourRating GetById(int id);
        int GenerateId();
        void Add(TourRating rating);
        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        bool IsRated(int id);
        List<TourRating> GetByTourId(int tourTimeId);
        void MarkAsInvalid(TourRating tourRating);
    }
}

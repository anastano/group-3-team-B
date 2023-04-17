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
        List<TourRating> GetAll();
        TourRating GetById(int id);
        void Add(TourRating rating);
        bool IsRated(int id);
        List<TourRating> GetByTourId(int tourTimeId);
        void MarkAsInvalid(TourRating tourRating);

        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}

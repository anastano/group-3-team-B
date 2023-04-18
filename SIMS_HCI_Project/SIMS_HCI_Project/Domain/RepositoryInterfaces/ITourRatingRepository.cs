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
        TourRating GetById(int id);
        List<TourRating> GetAll();
        List<TourRating> GetByTourId(int tourTimeId);

        void Add(TourRating rating);
        void Update(TourRating tourRating);

        bool IsRated(int id);

        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}

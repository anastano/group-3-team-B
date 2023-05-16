using SIMS_HCI_Project.Domain.Models;
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
    public class RatingGivenByGuestService
    {
        private readonly IRatingGivenByGuestRepository _ratingRepository;
        private readonly IUserRepository _userRepository;

        public RatingGivenByGuestService()
        {
            _ratingRepository = Injector.Injector.CreateInstance<IRatingGivenByGuestRepository>();
            _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
        }

        public RatingGivenByGuest GetById(int id)
        {
            return _ratingRepository.GetById(id);
        }

        public List<RatingGivenByGuest> GetAll()
        {
            return _ratingRepository.GetAll();
        }
        public RatingGivenByGuest GetByReservationId(int reservationId)
        {
            return _ratingRepository.GetByReservationId(reservationId);
        }
        public List<RatingGivenByGuest> GetByOwnerId(int ownerId)
        {
            return _ratingRepository.GetByOwnerId(ownerId);
        }
        public bool IsReservationRated(int reservationId)
        {
            return _ratingRepository.isReservationRated(reservationId);
        }

        public List<RatingGivenByGuest> GetRatedByOwnerId(RatingGivenByOwnerService ownerRatingService, int ownerId)
        {
            List<RatingGivenByGuest> ratedByOwnerId = new List<RatingGivenByGuest>();

            foreach (RatingGivenByGuest rating in GetByOwnerId(ownerId))
            {
                if (ownerRatingService.GetByReservationId(rating.ReservationId) != null)
                {
                    ratedByOwnerId.Add(rating);
                }
            }
            return ratedByOwnerId;
        }
        /// pogledati malo da li je ovo dobro definisano
        public RatingGivenByGuest RateReservation(AccommodationReservationService reservationService, RatingGivenByGuest rating)
        {
            RatingGivenByGuest addedRating = _ratingRepository.Add(rating);
            reservationService.GetById(rating.ReservationId).isRated = true;
            return addedRating;
            
        }
        public void ConnectRatingsWithReservations(AccommodationReservationService reservationService)
        {
            foreach (RatingGivenByGuest rating in _ratingRepository.GetAll())
            {
                rating.Reservation = reservationService.GetById(rating.ReservationId);
            }
        }

        public void FillAverageRatingAndSuperFlag(Owner owner)
        {
            FillAverageRating(owner);
            FillSuperFlag(owner);
        }

        public void FillAverageRating(Owner owner)
        {
            int ratingsSum = 0;
            int counter = 0;

            foreach (RatingGivenByGuest rating in GetByOwnerId(owner.Id))
            {
                ratingsSum += rating.Cleanliness + rating.Correctness;
                counter += 2;
            }

            owner.AverageRating = (double)ratingsSum / counter;
        }

        public void FillSuperFlag(Owner owner)
        {
            owner.SuperFlag = IsSuperFlag(owner) ? true : false;
        }

        public bool IsSuperFlag(Owner owner)
        {
            return (GetByOwnerId(owner.Id).Count >= 2 && owner.AverageRating > 4.5);
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

using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guest1 = SIMS_HCI_Project.Domain.Models.Guest1;

namespace SIMS_HCI_Project.Applications.Services
{
    public class RatingGivenByOwnerService
    {
        private readonly IRatingGivenByOwnerRepository _ratingRepository;

        public RatingGivenByOwnerService()
        {
            _ratingRepository = Injector.Injector.CreateInstance<IRatingGivenByOwnerRepository>();
        }

        public RatingGivenByOwner GetById(int id)
        {
            return _ratingRepository.GetById(id);
        }

        public RatingGivenByOwner GetByReservationId(int reservationId)
        {
            return _ratingRepository.GetByReservationId(reservationId);
        }
        public List<RatingGivenByOwner> GetByGuestId(int guestId)
        {
            return _ratingRepository.GetByGuestId(guestId);
        }
        public List<RatingGivenByOwner> GetAll()
        {
            return _ratingRepository.GetAll();
        }
        public double GetGuestAverageRate(Guest1 guest)
        {
            int ratingsSum = 0;
            int counter = 0;

            foreach (RatingGivenByOwner rating in GetByGuestId(guest.Id))
            {
                ratingsSum += rating.Cleanliness + rating.RuleCompliance;
                counter += 2;
            }
            return counter == 0 ? 0 : (double)ratingsSum / counter;
        }
        public List<AccommodationReservation> GetUnratedReservations(int ownerId, AccommodationReservationService reservationService)
        {
            List<AccommodationReservation> unratedReservations = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in reservationService.GetByOwnerId(ownerId))
            {
                if (reservationService.IsCompleted(reservation) && reservationService.IsWithinFiveDaysAfterCheckout(reservation) && !IsReservationRated(reservation))
                {
                    unratedReservations.Add(reservation);
                }
            }
            return unratedReservations;
        }
        public List<RatingGivenByOwner> GetRatedByGuestId(RatingGivenByGuestService guestRatingService, int guestId)
        {
            List<RatingGivenByOwner> ratedByOwnerId = new List<RatingGivenByOwner>();

            foreach (RatingGivenByOwner rating in GetByGuestId(guestId))
            {
                if (guestRatingService.GetByReservationId(rating.ReservationId) != null)
                {
                    ratedByOwnerId.Add(rating);
                }
            }
            return ratedByOwnerId;
        }

        public bool IsReservationRated(AccommodationReservation reservation)
        {
            return (GetByReservationId(reservation.Id) != null) ? true : false;
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
        /// <summary>
        /// dodajem novu metodu dodaj je u class diagram
        /// </summary>
        
        public List<KeyValuePair<int, int>> GetRatingStatisticsForCategory(int guestId, string categoryName)
        {
            List<KeyValuePair<int, int>> statstics = new List<KeyValuePair<int, int>>();
            for (int i = 1; i <= 5; i++)
            {
                statstics.Add(new KeyValuePair<int, int>(i, _ratingRepository.GetRatingCountForCategory(guestId, categoryName, i)));
            }
            return statstics;
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

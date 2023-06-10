using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGuestTourAttendanceRepository _guestTourAttendanceRepository;
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;

        public UserService() 
        { 
            _userRepository= Injector.Injector.CreateInstance<IUserRepository>();
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
            _accommodationReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
        }

        public User LogIn(string username, string password)
        {
            User user = _userRepository.GetByUsername(username);

            if (user == null || password != user.Password || !user.AccountActive) return null; // [Update] separate to return some Sucess DTO holding User info and Error info, to indicate what went wrong

            return user;
        }

        public bool SignIn(User newUser)
        {
            if (_userRepository.CheckIfUsernameExists(newUser.Username)) return false;

            _userRepository.Add(newUser);
            return true;
        }

        public void FillOwnerSuperFlag(RatingGivenByGuestService ratingService)
        {
            foreach (Owner owner in _userRepository.GetByUserRole(UserRole.OWNER))
            {
                ratingService.FillAverageRatingAndSuperFlag(owner);
            }
        }
        public bool HasUserBeenOnLocation(Location location, User user)
        {
            if(user.Role == UserRole.GUEST1)
            {
                return _accommodationReservationRepository.GetByGuestIdAndLocationId(user.Id, location.Id).Count != 0;
            }
            else if(user.Role == UserRole.GUEST2)
            {
                return _guestTourAttendanceRepository.GetAllByGuestAndLocationIds(user.Id, location.Id).Count != 0;
            }
            return true;

        }
    }
}

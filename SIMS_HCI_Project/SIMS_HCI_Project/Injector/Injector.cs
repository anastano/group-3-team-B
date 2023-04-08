using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Injector
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(IUserRepository), new UserRepository() },
            { typeof(IOwnerRepository), new OwnerRepository() },
            { typeof(IGuest1Repository), new Guest1Repository() },
            { typeof(ILocationRepository), new LocationRepository() },
            { typeof(IAccommodationRepository), new AccommodationRepository() },
             { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() }

        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}

using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IAccommodationRepository
    {
        Accommodation GetById(int id);
        List<Accommodation> GetAll();
        List<Accommodation> GetByOwnerId(int id);
        List<Accommodation> GetAllSortedBySuperFlag();
        List<Accommodation> Search(string name, string country, string city, string type, string guestsNumber, string reservationDays);
        List<string> GetImages(int id);
        void Delete(Accommodation accommodation);
        void Save();
        void Add(Accommodation accommodation);
    }
}
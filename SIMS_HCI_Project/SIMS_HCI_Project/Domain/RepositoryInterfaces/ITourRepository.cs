using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        Tour GetById(int id);
        List<Tour> GetAll();
        List<Tour> GetAllByGuide(int guideId);

        void Add(Tour tour);

        List<Tour> Search(string country, string city, int duration, string language, int guestsNum);
        public List<Tour> SearchByGuide(int guideId, string country, string city, int duration, string language, int guestsNum);
    }
}
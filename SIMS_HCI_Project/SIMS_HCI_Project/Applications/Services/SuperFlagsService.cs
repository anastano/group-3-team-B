using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    internal class SuperFlagsService
    {
        private readonly ITourTimeRepository _tourTimeRepository;
        private readonly ITourRatingRepository _tourRatingRepository;
        private readonly ISuperGuideFlagRepository _superGuideFlagRepository;

        public SuperFlagsService()
        {
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
            _tourRatingRepository = Injector.Injector.CreateInstance<ITourRatingRepository>();
            _superGuideFlagRepository = Injector.Injector.CreateInstance<ISuperGuideFlagRepository>();
        }

        public void ReviseGuideFlagStatusForLanguage(int guideId, string language)
        {
            int tourCount = _tourTimeRepository.GetLastYearCountByLanguage(guideId, language);

            if (tourCount < 10) return;

            List<TourRating> ratings = _tourRatingRepository.GetLastYearAllByLanguageAndGuide(guideId, language);
            if (ratings.Count() > 0 && ratings.Select(r => r.AverageRating).Average() > 4.9)
            {
                GiveSuperGuideFlag(guideId, language);
            }
        }

        private void GiveSuperGuideFlag(int guideId, string language)
        {
            SuperGuideFlag flag = new SuperGuideFlag(guideId, language, DateTime.Now, DateTime.Now.AddYears(1));

            _superGuideFlagRepository.Add(flag);
        }
    }
}

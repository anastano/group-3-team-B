using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SIMS_HCI_Project.Applications.Services
{
    internal class SuperFlagsService
    {
        private readonly ITourTimeRepository _tourTimeRepository;
        private readonly ITourRatingRepository _tourRatingRepository;
        private readonly ISuperGuideFlagRepository _superGuideFlagRepository;
        private readonly IUserRepository _userRepository;

        public SuperFlagsService()
        {
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
            _tourRatingRepository = Injector.Injector.CreateInstance<ITourRatingRepository>();
            _superGuideFlagRepository = Injector.Injector.CreateInstance<ISuperGuideFlagRepository>();
            _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
        }

        public void ReviseGuideFlagForLanguage(Guide guide, string language)
        {
            if (guide.GetSuperFlagByLanguage(language) != null) return;

            if(DoesFulfillRequirements(guide.Id, language))
            {
                GiveSuperGuideFlag(guide.Id, language);
            }
        }

        public void ReviseExpiredSuperGuideFlag(int flagId)
        {
            SuperGuideFlag flag = _superGuideFlagRepository.GetById(flagId);

            if (DoesFulfillRequirements(flag.GuideId, flag.Language))
            {
                flag.ExtendByYears(1);
                _superGuideFlagRepository.Update(flag);

                ((Guide)_userRepository.GetById(flag.GuideId)).SuperFlags.Add(flag);
            }
        }

        private bool DoesFulfillRequirements(int guideId, string language)
        {
            int tourCount = _tourTimeRepository.GetLastYearCountByLanguage(guideId, language);
            List<TourRating> ratings = _tourRatingRepository.GetLastYearAllByLanguageAndGuide(guideId, language);
            return tourCount > 10 && ratings.Count() > 0 && ratings.Select(r => r.AverageRating).Average() > 4.0;
        }

        private void GiveSuperGuideFlag(int guideId, string language)
        {
            SuperGuideFlag flag = new SuperGuideFlag(guideId, language, DateTime.Now, DateTime.Now.AddYears(1));

            _superGuideFlagRepository.Add(flag);

            ((Guide)_userRepository.GetById(flag.GuideId)).SuperFlags.Add(flag);
        }
    }
}

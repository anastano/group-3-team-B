using SIMS_HCI_Project.Domain.Models;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    internal interface ISuperGuideFlagRepository
    {
        SuperGuideFlag GetById(int id);
        SuperGuideFlag GetValidByGuide(int guideId);

        void Add(SuperGuideFlag flag);
        void Update(SuperGuideFlag flag);
    }
}
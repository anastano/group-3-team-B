using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Domain.RepositoryInterfaces
{
    internal interface ISuperGuideFlagRepository
    {
        SuperGuideFlag GetById(int id);
        List<SuperGuideFlag> GetValidByGuide(int guideId);

        void Add(SuperGuideFlag flag);
        void Update(SuperGuideFlag flag);
    }
}
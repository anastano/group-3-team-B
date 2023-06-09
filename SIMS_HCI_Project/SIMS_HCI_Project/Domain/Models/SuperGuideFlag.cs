using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    internal class SuperGuideFlag
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public string Language { get; set; }
        public DateTime AcquiredDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public SuperGuideFlag()
        {
            AcquiredDate = new DateTime();
            ExpiryDate = new DateTime();
        }

        public SuperGuideFlag(int guideId, string language, DateTime acquiredDate, DateTime expiryDate)
        {
            GuideId = guideId;
            Language = language;
            AcquiredDate = acquiredDate;
            ExpiryDate = expiryDate;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.DTOs
{
    public class AgeGroup
    {
        public int MinAge { get; set; }
        public int MaxAge { get; set; }

        public AgeGroup(int minAge, int maxAge)
        {
            MinAge = minAge;
            MaxAge = maxAge;
        }
    }
}

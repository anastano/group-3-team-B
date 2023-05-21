﻿using SIMS_HCI_Project.Serializer;
using SIMS_HCI_Project.WPF.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum UrgencyRenovationLevel
    {
        [Description("Level 1 - It would be nice to renovate some small things, but everything works fine without it.")]
        LEVEL1,

        [Description("Level 2 - Minor complaints about the accommodation that would make it perfect if addressed.")]
        LEVEL2,

        [Description("Level 3 - There are a few things that really bothered me and need to be renovated.")]
        LEVEL3,

        [Description("Level 4 - There are many bad things and renovation is really necessary.")]
        LEVEL4,

        [Description("Level 5 - The accommodation is in very bad condition and it's not worth renting unless it's renovated.")]
        LEVEL5
    }
    public class RenovationRecommendation
    {
        public int Id { get; set; }
        public int RatingId { get; set; }
        public RatingGivenByGuest Rating { get; set; }
        public UrgencyRenovationLevel UrgencyLevel { get; set; }
        public string Comment { get; set; }

        public RenovationRecommendation()
        {
            Comment = " ";
        }
        public RenovationRecommendation(string comment, UrgencyRenovationLevel level)
        {
            Comment = comment;
            UrgencyLevel = level;
        }
    }
}

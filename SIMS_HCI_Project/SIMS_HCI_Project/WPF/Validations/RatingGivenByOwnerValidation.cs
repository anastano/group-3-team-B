using iTextSharp.text.pdf.parser.clipper;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.Validations
{
    public class RatingGivenByOwnerValidation: ValidationBase
    {

        private int? _cleanliness;
        public int? Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (value != _cleanliness)
                {

                    _cleanliness = value;
                    OnPropertyChanged(nameof(Cleanliness));
                }
            }
        }

        private int? _ruleCompliance;
        public int? RuleCompliance
        {
            get => _ruleCompliance;
            set
            {
                if (value != _ruleCompliance)
                {

                    _ruleCompliance = value;
                    OnPropertyChanged(nameof(RuleCompliance));
                }
            }
        }

        public RatingGivenByOwnerValidation(){ }

        protected override void ValidateSelf()
        {
            if (Cleanliness == null)
            {
                this.ValidationErrors["Cleanliness"] = "Cleanliness is required";
            }
            else if (Cleanliness>5 || Cleanliness<1) 
            {
                this.ValidationErrors["Cleanliness"] = "Rating can be between 1 and 5";
            }

            if (RuleCompliance == null)
            {
                this.ValidationErrors["RuleCompliance"] = "Rule following is required";
            }
            else if (RuleCompliance > 5 || RuleCompliance < 1)
            {
                this.ValidationErrors["RuleCompliance"] = "Rating can be between 1 and 5";
            }
        }
    }
}

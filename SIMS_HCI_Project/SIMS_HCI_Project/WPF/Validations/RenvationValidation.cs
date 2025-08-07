using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.Validations
{
    public class RenvationValidation : ValidationBase
    {
        #region OnPropertyChanged

        private Accommodation _selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get => _selectedAccommodation;
            set
            {
                if (value != _selectedAccommodation)
                {
                    _selectedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedAccommodation));
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private int? _daysNumber;
        public int? DaysNumber
        {
            get => _daysNumber;
            set
            {
                if (value != _daysNumber)
                {
                    _daysNumber = value;
                    OnPropertyChanged(nameof(DaysNumber));
                }
            }
        }

        private DateTime _enteredStart;
        public DateTime EnteredStart
        {
            get => _enteredStart;
            set
            {
                if (value != _enteredStart)
                {
                    _enteredStart = value;
                    OnPropertyChanged(nameof(EnteredStart));
                }
            }
        }

        private DateTime _enteredEnd;
        public DateTime EnteredEnd
        {
            get => _enteredEnd;
            set
            {
                if (value != _enteredEnd)
                {
                    _enteredEnd = value;
                    OnPropertyChanged(nameof(EnteredEnd));
                }
            }
        }

        #endregion

        public RenvationValidation() 
        {
            EnteredStart = DateTime.Today.AddDays(1);
            EnteredEnd = DateTime.Today.AddDays(1);
        }

        protected override void ValidateSelf()
        {
            if (SelectedAccommodation == null)
            {
                this.ValidationErrors["SelectedAccommodation"] = "Accommodation is required";
            }

            if (string.IsNullOrEmpty(Description))
            {
                this.ValidationErrors["Description"] = "Description is required";
            }

            if (DaysNumber == null)
            {
                this.ValidationErrors["DaysNumber"] = "Number of days is required";
            }

            if (DaysNumber < 1)
            {
                this.ValidationErrors["DaysNumber"] = "Number of days must be greater than zero";
            }

            if (EnteredStart <= DateTime.Today)
            {
                this.ValidationErrors["EnteredStart"] = "Date must be after today";
            }

            if (EnteredEnd <= DateTime.Today)
            {
                this.ValidationErrors["EnteredEnd"] = "Date must be after today";
            }

            if (EnteredStart > EnteredEnd)
            {
                this.ValidationErrors["EnteredStart"] = "Start date must be before the end date";
            }


        }
    }
}

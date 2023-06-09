using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.Validations
{
    public class RegularTourRequestValidation : ValidationBase
    {
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {

                    _city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {

                    _country = value;
                    OnPropertyChanged(nameof(Country));
                }
            }
        }

        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {

                    _language = value;
                    OnPropertyChanged(nameof(Language));
                }
            }
        }

        private int? _guestNumber;
        public int? GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (value != _guestNumber)
                {

                    _guestNumber = value;
                    OnPropertyChanged(nameof(GuestNumber));
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

        public RegularTourRequestValidation()
        {
            EnteredStart = DateTime.Now.AddDays(3); //48h before requests turns invalid
            EnteredEnd = DateTime.Now.AddDays(4);
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrEmpty(City))
            {
                this.ValidationErrors["City"] = "City is required";
            }

            if (string.IsNullOrEmpty(Country))
            {
                this.ValidationErrors["Country"] = "Country is required";
            }

            if (string.IsNullOrEmpty(Language))
            {
                this.ValidationErrors["Language"] = "Language is required";
            }

            if (string.IsNullOrEmpty(Description))
            {
                this.ValidationErrors["Description"] = "Description is required";
            }

            if (GuestNumber == null)
            {
                this.ValidationErrors["GuestNumber"] = "Guest number is required";
            }
            else if (GuestNumber < 1)
            {
                this.ValidationErrors["GuestNumber"] = "Guest number must be number greater than zero";
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

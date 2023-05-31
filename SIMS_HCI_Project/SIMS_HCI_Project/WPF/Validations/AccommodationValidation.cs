using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.Validations
{
    public class AccommodationValidation : ValidationBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {

                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

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

        private int? _maxGuests;
        public int? MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (value != _maxGuests)
                {

                    _maxGuests = value;
                    OnPropertyChanged(nameof(MaxGuests));
                }
            }
        }

        private int? _minDays;
        public int? MinDays
        {
            get => _minDays;
            set
            {
                if (value != _minDays)
                {

                    _minDays = value;
                    OnPropertyChanged(nameof(MinDays));
                }
            }
        }

        private int? _cancellationDays;
        public int? CancellationDays
        {
            get => _cancellationDays;
            set
            {
                if (value != _cancellationDays)
                {

                    _cancellationDays = value;
                    OnPropertyChanged(nameof(CancellationDays));
                }
            }
        }


        public AccommodationValidation() { }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrEmpty(Name))
            {
                this.ValidationErrors["Name"] = "Name is required";
            }

            if (string.IsNullOrEmpty(City))
            {
                this.ValidationErrors["City"] = "City is required";
            }

            if (string.IsNullOrEmpty(Country))
            {
                this.ValidationErrors["Country"] = "Country is required";
            }

            if (MaxGuests == null)
            {
                this.ValidationErrors["MaxGuests"] = "Maximum guests is required";
            }
            else if (MaxGuests < 1)
            {
                this.ValidationErrors["MaxGuests"] = "Maximum guests must be number greater than zero";
            }

            if (MinDays == null)
            {
                this.ValidationErrors["MinDays"] = "Minimum days is required";
            }
            else if (MinDays < 1)
            {
                this.ValidationErrors["MinDays"] = "Minimum days must be number greater than zero";
            }

            if (CancellationDays == null)
            {
                this.ValidationErrors["CancellationDays"] = "Cancellation days is required";
            }
            else if (CancellationDays < 1)
            {
                this.ValidationErrors["CancellationDays"] = "Cancellation days must be number greater than zero";
            }

        }
    }
}

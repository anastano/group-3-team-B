using SIMS_HCI_Project.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Model
{
    public class Location : ISerializable, IDataErrorInfo
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public Location() { }

        public Location(string city, string country)
        {
            City = city;
            Country = country;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), City, Country };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            City = values[1];
            Country = values[2];
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "City":
                        if (string.IsNullOrEmpty(City))
                            return "City is required";
                        break;
                    case "Country":
                        if (string.IsNullOrEmpty(Country))
                            return "Country is required";
                        break;
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "City", "Country"};

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

    }
}

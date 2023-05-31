using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.Validations
{
    public class ImageURLValidation : ValidationBase
    {
        private string _imageURL;
        public string ImageURL
        {
            get => _imageURL;
            set
            {
                if (value != _imageURL)
                {

                    _imageURL = value;
                    OnPropertyChanged(nameof(ImageURL));
                }
            }
        }

        public ImageURLValidation() { }

        protected override void ValidateSelf()
        {
            Regex regexURL = new Regex("(http(s?)://.)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)|(^$)");

            if (string.IsNullOrWhiteSpace(ImageURL))
            {
                this.ValidationErrors["ImageURL"] = "URL can't be empty.";
            }
            else if (!regexURL.IsMatch(ImageURL))
            {
                this.ValidationErrors["ImageURL"] = "URL is not in valid format.";
            }
        }
    }
}




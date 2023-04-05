using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for AccommodationRegistrationView.xaml
    /// </summary>
    public partial class AccommodationRegistrationView : Window, INotifyPropertyChanged
    {
        public Owner Owner { get; set; }

        public Accommodation Accommodation { get; set; }
        public Location Location { get; set; }

        private AccommodationController _accommodationController;

        private string _imageURL;
        public string ImageURL
        {
            get { return _imageURL; }
            set
            {
                _imageURL = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Images { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public AccommodationRegistrationView(AccommodationController accommodationController, Owner owner)
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner;
            
            Accommodation = new Accommodation();
            Location = new Location();
            Images = new ObservableCollection<string>();
            ImageURL = "";

            _accommodationController = accommodationController;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
             Accommodation.OwnerId = Owner.Id;
             Accommodation.Images = new List<string>(Images);

             _accommodationController.Register(Accommodation, Location);

             Close();            
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
              if (!ImageURL.Equals(""))
              {
                   Images.Add(ImageURL);
                   ImageURL = "";
              }
        }

        private void btnRemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if (lbImages.SelectedItem != null)
            {
                Images.RemoveAt(lbImages.SelectedIndex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Insert))
                btnAddImage_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.Delete))
                btnRemoveImage_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R))
                btnRegister_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.Escape))
                btnCancel_Click(sender, e);
        }

    }
}

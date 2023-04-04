using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for AccommodationView.xaml
    /// </summary>
    public partial class OwnerAccommodationsView : Window, IObserver
    {
        public Owner Owner { get; set; }

        private AccommodationController _accommodationController;
        private OwnerController _ownerController;

        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public OwnerAccommodationsView(AccommodationController accommodationController, OwnerController ownerController, Owner owner)
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner;

            _accommodationController = accommodationController;
            _ownerController = ownerController;

            Accommodations = new ObservableCollection<Accommodation>(_ownerController.FindById(Owner.Id).Accommodations);

            _accommodationController.Subscribe(this);
        }

        public void Update()
        {
            UpdateAccommodations();
        }

        public void UpdateAccommodations()
        {          
            Accommodations.Clear();
            foreach (Accommodation accommodation in _ownerController.FindById(Owner.Id).Accommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void btnAddAccommodation_Click(object sender, RoutedEventArgs e)
        {
            Window accommodationRegistration = new AccommodationRegistrationView(_accommodationController, Owner);
            accommodationRegistration.Show();
        }

        private void btnDeleteAccommodation_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                MessageBoxResult confirmationResult = ConfirmAccommodationDeletion();

                if (confirmationResult == MessageBoxResult.Yes)
                {
                    _accommodationController.Remove(SelectedAccommodation);
                }
            }
        }

        private MessageBoxResult ConfirmAccommodationDeletion() { 
        
            string sMessageBoxText = $"Are you sure you want to delete accommodation '{SelectedAccommodation.Name}'?";
            string sCaption = "Confirmation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private void btnShowImages_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                Window accommodationImages = new OwnerAccommodationImagesView(_accommodationController, SelectedAccommodation);
                accommodationImages.Show();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
                btnAddAccommodation_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D))
                btnDeleteAccommodation_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
                btnShowImages_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.Escape))
                btnClose_Click(sender, e);
        }


    }
}

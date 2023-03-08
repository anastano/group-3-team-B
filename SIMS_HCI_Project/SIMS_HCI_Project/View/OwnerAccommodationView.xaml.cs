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
    public partial class AccommodationView : Window, IObserver
    {
        private string _ownerId;

        private AccommodationController _accommodationController;
        public ObservableCollection<Accommodation> Accommodations { get; set; }

        public Accommodation SelectedAccommodation;

        public AccommodationView(AccommodationController accommodationController, string ownerId)
        {
            InitializeComponent();
            DataContext = this;

            _ownerId = ownerId;

            _accommodationController = accommodationController;

            Accommodations = new ObservableCollection<Accommodation>(_accommodationController.GetAllByOwnerId(_ownerId));
            accommodationController.Subscribe(this);
            _ownerId = ownerId;
        }

        public void Update()
        {

        }
    }
}

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
        private string _ownerId;

        private AccommodationController _accommodationController;
        private OwnerController _ownerController;

        public ObservableCollection<Accommodation> Accommodations { get; set; }

        public Accommodation SelectedAccommodation { get; set; }

        public OwnerAccommodationsView(AccommodationController accommodationController, OwnerController ownerController, string ownerId)
        {
            InitializeComponent();
            DataContext = this;

            _ownerId = ownerId;

            _accommodationController = accommodationController;
            _ownerController = ownerController;

            Accommodations = new ObservableCollection<Accommodation>(_ownerController.GetAccommodations(_ownerId));

            _accommodationController.Subscribe(this); //its method Add contains adding accommodation to owner accommodation list
        }

        public void Update()
        {

        }
    }
}

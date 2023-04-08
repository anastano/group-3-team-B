using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
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

namespace SIMS_HCI_Project.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for AccommodationsView.xaml
    /// </summary>
    public partial class AccommodationsView : Window, IObserver
    {
        public Owner Owner { get; set; }

        private readonly AccommodationService _accommodationService;
        private readonly OwnerService _ownerService;
             
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public AccommodationsView(AccommodationService accommodationService, OwnerService ownerService, Owner owner)
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner;

            _ownerService= ownerService;
            _accommodationService = accommodationService;

            Accommodations = new ObservableCollection<Accommodation>(_ownerService.FindById(Owner.Id).Accommodations);

            _accommodationService.Subscribe(this);
        }

        public void Update()
        {

        }
    }
}

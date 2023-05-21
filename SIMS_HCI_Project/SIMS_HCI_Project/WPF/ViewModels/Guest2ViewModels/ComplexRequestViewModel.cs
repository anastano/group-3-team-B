using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class ComplexRequestViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public RelayCommand CreateRequest { get; set; }
        public NavigationService NavigationService { get; set; }
        private RegularTourRequestService _regularTourRequestService { get; set; }
        public ComplexRequestsView ComplexRequestsView { get; set; }
        public Guest2 Guest { get; set; }

        private ObservableCollection<RegularTourRequest> _myRequests;
        public ObservableCollection<RegularTourRequest> MyRequests //nece biti lista RegularTourRequest, vec ComplexTourRequest, koji ce sadrzati List<int> RegularTourRequestsIdsWhichArePArtOfComplex, Id gosta, Submissiom date, i id complexRequest-a, mozda moze da sadrzi i listu citavih objekata RegularTourRequest koji su njegovi delovi, radi lakseg bindovanja
        {
            get { return _myRequests; }
            set
            {
                _myRequests = value;
                OnPropertyChanged();
            }
        }

        private Frame _createRequestFrame;
        public Frame CreateRequestFrame
        {
            get { return _createRequestFrame; }
            set
            {
                _createRequestFrame = value;
                OnPropertyChanged();
            }
        }


        public ComplexRequestViewModel(ComplexRequestsView complexRequestsView, Guest2 guest2, NavigationService navigationService, Frame createRequestFrame)
        {
            ComplexRequestsView = complexRequestsView;
            Guest = guest2;
            NavigationService = navigationService;
            CreateRequestFrame = createRequestFrame;

            LoadFromFiles();
            InitCommands();

            MyRequests = new ObservableCollection<RegularTourRequest>(_regularTourRequestService.GetAllByGuestId(Guest.Id, false));
        }

        private void InitCommands()
        {
            CreateRequest = new RelayCommand(ExecuteCreateRequest, CanExecute);
        }

        private void ExecuteCreateRequest(object obj)
        {
            CreateRequestFrame.NavigationService.Navigate(new CreateRegularRequestView(Guest, NavigationService)); //ovo neka ostane, posto je forma ista kao za regularan zahtev, samo sto se prosledjuje flag IsPartOfComplex=true, malo mozda izmeniti kao da pise koliko delova je dodao i koji je ovo po redu
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        private void LoadFromFiles()
        {
            _regularTourRequestService = new RegularTourRequestService();
        }
    }
}

using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationsViewModel: INotifyPropertyChanged
    {

        private readonly AccommodationService _accommodationService;
        public AccommodationsView AccommodationsView { get; set; }
        public Owner Owner { get; set; }   
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public Style NormalButtonStyle { get; set; }
        public Style SelectedButtonStyle { get; set; }
        public Style NormalRowStyle { get; set; }
        public  Style SelectedRowStyle { get; set; }

        #region OnPropertyChanged

        private Style _addAccommodationButtonStyle;
        public Style AddAccommodationButtonStyle
        {
            get => _addAccommodationButtonStyle;
            set
            {
                if (value != _addAccommodationButtonStyle)
                {

                    _addAccommodationButtonStyle = value;
                    OnPropertyChanged(nameof(AddAccommodationButtonStyle));
                }
            }
        }

        private Style _showImageButtonStyle;
        public Style ShowImageButtonStyle
        {
            get => _showImageButtonStyle;
            set
            {
                if (value != _showImageButtonStyle)
                {

                    _showImageButtonStyle = value;
                    OnPropertyChanged(nameof(ShowImageButtonStyle));
                }
            }
        }

        private Style _showSuggestionsButtonStyle;
        public Style ShowSuggestionsButtonStyle
        {
            get => _showSuggestionsButtonStyle;
            set
            {
                if (value != _showSuggestionsButtonStyle)
                {

                    _showSuggestionsButtonStyle = value;
                    OnPropertyChanged(nameof(ShowSuggestionsButtonStyle));
                }
            }
        }

        private Style _closeButtonStyle;
        public Style CloseButtonStyle
        {
            get => _closeButtonStyle;
            set
            {
                if (value != _closeButtonStyle)
                {

                    _closeButtonStyle = value;
                    OnPropertyChanged(nameof(CloseButtonStyle));
                }
            }
        }

        private Style _rowStyle;
        public Style RowStyle
        {
            get => _rowStyle;
            set
            {
                if (value != _rowStyle)
                {

                    _rowStyle = value;
                    OnPropertyChanged(nameof(RowStyle));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public RelayCommand AddAccommodationCommand { get; set; }
        public RelayCommand ShowAccommodationImagesCommand { get; set; }
        public RelayCommand ShowSuggestionsCommand { get; set; }
        public RelayCommand CloseAccommodationsViewCommand { get; set; }
        public RelayCommand StopDemoCommand { get; set; }

        public AccommodationsViewModel(AccommodationsView accommodationsView, Owner owner)
        {
            InitCommands();

            _accommodationService = new AccommodationService();

            AccommodationsView = accommodationsView;
            Owner = owner;     
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetByOwnerId(Owner.Id));

            NormalButtonStyle = Application.Current.FindResource("OwnerButtonStyle") as Style;
            SelectedButtonStyle = Application.Current.FindResource("OwnerSelectedButtonStyle") as Style;
            NormalRowStyle = null;
            RowStyle = NormalRowStyle;
            SelectedRowStyle = Application.Current.FindResource("OwnerSelectedDataGridRow") as Style;
            AddAccommodationButtonStyle = NormalButtonStyle;
            ShowImageButtonStyle = NormalButtonStyle;
            ShowSuggestionsButtonStyle = NormalButtonStyle;
            CloseButtonStyle = NormalButtonStyle;

            CancellationToken CT = OwnerMainViewModel.CTS.Token;
            DemoIsOn(CT);

        }

        #region DemoIsOn
        private async Task DemoIsOn(CancellationToken CT) {
            if (OwnerMainViewModel.Demo) {
                //demo message - add accommodation
                await Task.Delay(1000, CT);
                Window messageView1 = new MessageView("The first feature is the registration of a new accommodation.", "Stop Demo Mode (Ctrl+Q)");
                messageView1.Show();
                await Task.Delay(3500, CT);
                messageView1.Close();
                await Task.Delay(1500, CT);

                //add accommodation
                AddAccommodationButtonStyle = SelectedButtonStyle;
                await Task.Delay(1500, CT);;
                AddAccommodationButtonStyle = NormalButtonStyle;
                Window addAccommodationView = new AddAccommodationView(this, Owner);
                addAccommodationView.ShowDialog();
                await Task.Delay(1500, CT);

                //demo message - show images
                Window messageView3 = new MessageView("The next feature is the display of accommodation images.", "Stop Demo Mode (Ctrl+Q)");
                messageView3.Show();
                await Task.Delay(3500, CT);
                messageView3.Close();
                await Task.Delay(1500, CT);

                //show images
               
                RowStyle = SelectedRowStyle;
                await Task.Delay(1500, CT);
                ShowImageButtonStyle = SelectedButtonStyle;
                await Task.Delay(1500, CT);
                RowStyle = NormalRowStyle;
                ShowImageButtonStyle = NormalButtonStyle;
                Window accommodationImagesView = new AccommodationImagesView(AccommodationsView, Accommodations.FirstOrDefault());
                accommodationImagesView.ShowDialog();
                await Task.Delay(1500, CT);

                //demo message - show suggestions
                Window messageView4 = new MessageView("The next feature is the display of suggestions.", "Stop Demo Mode (Ctrl+Q)");
                messageView4.Show();
                await Task.Delay(3500, CT);
                messageView4.Close();
                await Task.Delay(1500, CT);

                //show suggestions
                ShowSuggestionsButtonStyle = SelectedButtonStyle;
                await Task.Delay(1500, CT);
                ShowSuggestionsButtonStyle = NormalButtonStyle;
                Window accommodationSuggestions = new AccommodationSuggestionsView(AccommodationsView, Owner);
                accommodationSuggestions.ShowDialog();
                await Task.Delay(1500, CT);

                //close window
                CloseButtonStyle = SelectedButtonStyle;
                await Task.Delay(1500, CT);
                CloseButtonStyle = NormalButtonStyle;
                AccommodationsView.Close();
            }
        }
        #endregion

        #region Commands

        public void Executed_AddAccommodationCommand(object obj)
        {
            Window addAccommodationView = new AddAccommodationView(this, Owner);
            addAccommodationView.ShowDialog();
        }

        public bool CanExecute_AddAccommodationCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowAccommodationImagesCommand(object obj)
        {
            if (SelectedAccommodation != null)
            {
                Window accommodationImagesView = new AccommodationImagesView(AccommodationsView , SelectedAccommodation);
                accommodationImagesView.ShowDialog();
            }
            else
            {
                MessageBox.Show("No accommodation has been selected");
            }
        }

        public bool CanExecute_ShowAccommodationImagesCommand(object obj)
        {
            return true;
        }


        public void Executed_ShowSuggestionsCommand(object obj)
        {
            Window accommodationSuggestionsView = new AccommodationSuggestionsView(AccommodationsView, Owner);
            accommodationSuggestionsView.ShowDialog();
        }

        public bool CanExecute_ShowSuggestionsCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseAccommodationsViewCommand(object obj)
        {
            AccommodationsView.Close();
        }

        public bool CanExecute_CloseAccommodationsViewCommand(object obj)
        {
            return true;
        }

        private async Task StopDemo()
        {
            if (OwnerMainViewModel.Demo)
            {
                OwnerMainViewModel.CTS.Cancel();
                OwnerMainViewModel.Demo = false;

                //demo message - end
                AccommodationsView.Close();
                Window messageDemoOver = new MessageView("The demo mode is over.", "");
                messageDemoOver.Show();
                await Task.Delay(2500);
                messageDemoOver.Close();
            }
        }

        public void Executed_StopDemoCommand(object obj)
        {
            try
            {
                StopDemo();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Error!");
            }
        }

        public bool CanExecute_StopDemoCommand(object obj)
        {
            return true;
        }


        #endregion

        public void InitCommands()
        {
            AddAccommodationCommand = new RelayCommand(Executed_AddAccommodationCommand, CanExecute_AddAccommodationCommand);
            ShowAccommodationImagesCommand = new RelayCommand(Executed_ShowAccommodationImagesCommand, CanExecute_ShowAccommodationImagesCommand);
            ShowSuggestionsCommand = new RelayCommand(Executed_ShowSuggestionsCommand, CanExecute_ShowSuggestionsCommand);
            CloseAccommodationsViewCommand = new RelayCommand(Executed_CloseAccommodationsViewCommand, CanExecute_CloseAccommodationsViewCommand);
            StopDemoCommand = new RelayCommand(Executed_StopDemoCommand, CanExecute_StopDemoCommand);
        }

        public void UpdateAccommodations()
        {
            Accommodations.Clear();
            foreach (Accommodation accommodation in _accommodationService.GetByOwnerId(Owner.Id))
            {
                Accommodations.Add(accommodation);
            }
        }
    }
}

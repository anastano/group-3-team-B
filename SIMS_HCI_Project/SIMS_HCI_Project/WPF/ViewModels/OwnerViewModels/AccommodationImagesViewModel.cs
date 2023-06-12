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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationImagesViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationService _accommodationService;
        private int _currentImageIndex;
        public Accommodation Accommodation { get; set; }
        public AccommodationImagesView AccommodationImagesView { get; set; }
        public AccommodationsView AccommodationsView { get; set; }
        public Style NormalButtonStyle { get; set; }
        public Style SelectedButtonStyle { get; set; }

        #region OnPropertyChanged
        public List<string> Images { get; set; }

        private string _image;
        public string Image
        {
            get => _image;
            set
            {
                if (value != _image)
                {
                    _image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }

        private Style _nextButtonStyle;
        public Style NextButtonStyle
        {
            get => _nextButtonStyle;
            set
            {
                if (value != _nextButtonStyle)
                {
                    _nextButtonStyle = value;
                    OnPropertyChanged(nameof(NextButtonStyle));
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public RelayCommand PreviousImageCommand { get; set; }
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand StopDemoCommand { get; set; }  

        public AccommodationImagesViewModel(AccommodationImagesView imagesView, AccommodationsView accommodationsView, Accommodation accommodation)
        {
            InitCommands();

            _accommodationService = new AccommodationService();

            AccommodationImagesView = imagesView;
            AccommodationsView = accommodationsView;

            Accommodation = accommodation;
            Images = _accommodationService.GetImages(Accommodation.Id);
            _currentImageIndex = 0;

            if (Images.Count != 0)
            {
                Image = Images[_currentImageIndex];
            }

            NormalButtonStyle = Application.Current.FindResource("OwnerButtonStyle") as Style;
            SelectedButtonStyle = Application.Current.FindResource("OwnerSelectedButtonStyle") as Style;
            NextButtonStyle = NormalButtonStyle;
            CloseButtonStyle = NormalButtonStyle;

            DemoIsOn();
        }

        #region DemoIsOn
        private async Task DemoIsOn()
        {
            if (OwnerMainViewModel.Demo)
            {
                await Task.Delay(1000);
                NextButtonStyle = SelectedButtonStyle;
                await Task.Delay(1500);
                NextButtonStyle = NormalButtonStyle;
                _currentImageIndex++;
                ChangeOutrangeCurrentImageIndex();
                Image = Images[_currentImageIndex];
                await Task.Delay(1500);
                CloseButtonStyle = SelectedButtonStyle;
                await Task.Delay(1500);
                CloseButtonStyle = NormalButtonStyle;
                AccommodationImagesView.Close();
            }
        }
        #endregion

        private void ChangeOutrangeCurrentImageIndex()
        {
            if (_currentImageIndex < 0)
            {
                _currentImageIndex = Images.Count - 1;
            }
            else if (_currentImageIndex >= Images.Count)
            {
                _currentImageIndex = 0;
            }
        }

        #region Commands
        public void Executed_PreviousImageCommand(object obj)
        {
            _currentImageIndex--;
            ChangeOutrangeCurrentImageIndex();
            if (Images.Count != 0)
            {
                Image = Images[_currentImageIndex];
            }
        }

        public void Executed_NextImageCommand(object obj)
        {
            _currentImageIndex++;
            ChangeOutrangeCurrentImageIndex();
            if (Images.Count != 0)
            {
                Image = Images[_currentImageIndex];
            }
        }

        public void Executed_CloseViewCommand(object obj)
        {
            AccommodationImagesView.Close();
        }

        private async Task StopDemo()
        {
            if (OwnerMainViewModel.Demo)
            {
                OwnerMainViewModel.CTS.Cancel();
                OwnerMainViewModel.Demo = false;

                //demo message - end
                AccommodationImagesView.Close();
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

        #endregion

        public void InitCommands()
        {
            PreviousImageCommand = new RelayCommand(Executed_PreviousImageCommand);
            NextImageCommand = new RelayCommand(Executed_NextImageCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
            StopDemoCommand = new RelayCommand(Executed_StopDemoCommand);
        }
    }
}

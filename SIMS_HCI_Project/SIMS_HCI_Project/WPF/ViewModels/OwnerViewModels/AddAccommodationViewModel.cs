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
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class AddAccommodationViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationService _accommodationService;
        public Owner Owner { get; set; }
        public Accommodation Accommodation { get; set; }
        public Location Location { get; set; }
        public AddAccommodationView AddAccommodationView { get; set; }

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

        public RelayCommand AddAccommodationImageCommand { get; set; }
        public RelayCommand RemoveAccommodationImageCommand { get; set; }
        public RelayCommand RegisterNewAccommodationCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public AddAccommodationViewModel(AddAccommodationView addAccommodationView, AccommodationService accommodationService, Owner owner)
        {
            InitCommands();

            _accommodationService = accommodationService;
            AddAccommodationView = addAccommodationView;

            Owner = owner;
            Accommodation = new Accommodation();
            Location = new Location();
            Images = new ObservableCollection<string>();
            ImageURL = "";
        }

        #region Commands
        public void Executed_AddAccommodationImageCommand(object obj)
        {
            if (!ImageURL.Equals(""))
            {
                Images.Add(ImageURL);
                ImageURL = "";
            }
        }

        public bool CanExecute_AddAccommodationImageCommand(object obj)
        {
            return true;
        }

        public void Executed_RemoveAccommodationImageCommand(object obj)
        {
            if (AddAccommodationView.lbImages.SelectedItem != null)
            {
                Images.RemoveAt(AddAccommodationView.lbImages.SelectedIndex);
            }
        }

        public bool CanExecute_RemoveAccommodationImageCommand(object obj)
        {
            return true;
        }

        public void Executed_RegisterNewAccommodationCommand(object obj)
        {
            Accommodation.OwnerId = Owner.Id;
            Accommodation.Images = new List<string>(Images);

           _accommodationService.Add(Accommodation, Location);

            AddAccommodationView.Close();
        }

        public bool CanExecute_RegisterNewAccommodationCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            AddAccommodationImageCommand = new RelayCommand(Executed_AddAccommodationImageCommand, CanExecute_AddAccommodationImageCommand);
            RemoveAccommodationImageCommand = new RelayCommand(Executed_RemoveAccommodationImageCommand, CanExecute_RemoveAccommodationImageCommand);
            RegisterNewAccommodationCommand = new RelayCommand(Executed_RegisterNewAccommodationCommand, CanExecute_RegisterNewAccommodationCommand);
        }

        }
    }

using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ForumViewModel
    {
        private ForumService _forumService;
        private NavigationService _navigationService;
        public Guest1 Guest { get; set; }
        public Forum Forum { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand AddCommentCommand { get; set; }
        public int SelectedTab { get; set; }
        public ForumViewModel(Guest1 guest, NavigationService navigationService, int forumId, int selectedTab)
        {
            _forumService = new ForumService();
            _navigationService = navigationService;
            SelectedTab = selectedTab;
            Guest = guest;
            Forum = _forumService.GetById(forumId);
            BackCommand = new RelayCommand(ExecutedBackCommand, CanExecute);
            AddCommentCommand = new RelayCommand(ExecutedAddCommentCommand, CanExecute);
        }
        public void ExecutedBackCommand(object obj)
        {
            _navigationService.Navigate(new ForumsViewModel(Guest, _navigationService, SelectedTab), "Forums");
        }
        public void ExecutedAddCommentCommand(object obj)
        {
            //_navigationService.NavigateBack();
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
    }
}

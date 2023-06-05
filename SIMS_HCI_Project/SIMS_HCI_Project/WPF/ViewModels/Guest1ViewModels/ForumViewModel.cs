using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ForumViewModel : INotifyPropertyChanged
    {
        private ForumService _forumService;
        private ForumCommentService _forumCommentService;
        private NavigationService _navigationService;
        public Guest1 Guest { get; set; }
        public Forum Forum { get; set; }
        public ObservableCollection<ForumComment> ForumComments { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand AddCommentCommand { get; set; }
        public int SelectedTab { get; set; }
        private string _usefulMessage;
        public string UsefulMessage
        {
            get => _usefulMessage;
            set
            {
                if (value != _usefulMessage)
                {

                    _usefulMessage = value;
                    OnPropertyChanged(nameof(UsefulMessage));
                }
            }
        }
        private bool _isForumUseful;
        public bool IsForumUseful
        {
            get => _isForumUseful;
            set
            {
                if (value != _isForumUseful)
                {

                    _isForumUseful = value;
                    OnPropertyChanged(nameof(IsForumUseful));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ForumViewModel(Guest1 guest, NavigationService navigationService, Forum forum, int selectedTab)
        {
            _forumService = new ForumService();
            _forumCommentService = new ForumCommentService();
            _navigationService = navigationService;
            SelectedTab = selectedTab;
            Guest = guest;
            Forum = forum;
            UsefulMessage = "Very Useful";
            IsForumUseful = _forumService.IsUSeful(Forum);
            Forum = _forumService.GetById(forum.Id);
            ForumComments = new ObservableCollection<ForumComment>(_forumCommentService.GetByForumId(Forum.Id));
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

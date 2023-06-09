using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class AddForumCommentViewModel : INotifyPropertyChanged
    {
        private NavigationService _navigationService;
        private ForumCommentService _forumCommentService;
        private UserService _userService;
        public RelayCommand PostCommentCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public Forum Forum { get; set; }
        public Guest1 Guest { get; set; }
        private String _comment;
        public String Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }
        private String _errorMessage;
        public String ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (value != _errorMessage)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }
        private String _user;
        public String User
        {
            get => _user;
            set
            {
                if (value != _user)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isErrorVisible;
        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set
            {
                if (value != _isErrorVisible)
                {
                    _isErrorVisible = value;
                    OnPropertyChanged();
                }
            }
        }


        private Regex urlRegex = new Regex("(http(s?)://.)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)|(^$)");

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AddForumCommentViewModel(Guest1 guest, Forum forum, NavigationService navigationService)
        {
            _navigationService = navigationService;
            _forumCommentService = new ForumCommentService();
            _userService = new UserService();
            Forum = forum;
            Guest = guest;
            User = guest.GetFullName();
            Comment = "";
            IsErrorVisible = false;
            InitCommands();
        }
        public void ExecutedPostCommentCommand(object obj)
        {
            if (!(Comment == ""))
            {
                bool isUseful = _userService.HasUserBeenOnLocation(Forum.Location, Guest);
                _forumCommentService.Add(new ForumComment(Guest, Forum, Comment, isUseful));
                _navigationService.NavigateBack();
            }
            ErrorMessage = "Please type comment";
            IsErrorVisible = true;

        }
        public void ExecutedCancelCommand(object obj)
        {
            _navigationService.NavigateBack();
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            PostCommentCommand = new RelayCommand(ExecutedPostCommentCommand, CanExecute);
            CancelCommand = new RelayCommand(ExecutedCancelCommand, CanExecute);
        }
    }
}

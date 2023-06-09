using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Validations;
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

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class LeaveCommentViewModel : INotifyPropertyChanged
    {
        private readonly ForumCommentService _forumCommentService;
        
        public LeaveCommentView LeaveCommentView { get; set; }
        public SelectedForumViewModel SelectedForumVM { get; set; }
        
        public Owner Owner { get; set; }
        public Forum Forum { get; set; }

        #region OnPropertyChanged

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {

                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public RelayCommand SubmitCommentCommand { get; set; }
        public RelayCommand CloseLeaveCommentViewCommand { get; set; }


        public LeaveCommentViewModel(LeaveCommentView leaveCommentView, SelectedForumViewModel selectedForumVM, Owner owner, Forum forum)
        {
            InitCommands();

            _forumCommentService = new ForumCommentService();

            LeaveCommentView = leaveCommentView;
            SelectedForumVM = selectedForumVM;
            Owner = owner;
            Forum = forum;
        }

        #region Commands
        public void Executed_SubmitCommentCommand(object obj)
        {          
            ForumComment comment = new ForumComment(Owner, Forum, Comment, true);
            _forumCommentService.Add(comment);
            SelectedForumVM.UpdateComments();
            LeaveCommentView.Close();           
        }

        public bool CanExecute_SubmitCommentCommand(object obj)
        {
            return true;
        }


        public void Executed_CloseLeaveCommentViewCommand(object obj)
        {
            LeaveCommentView.Close();
        }

        public bool CanExecute_CloseLeaveCommentViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            SubmitCommentCommand = new RelayCommand(Executed_SubmitCommentCommand, CanExecute_SubmitCommentCommand);
            CloseLeaveCommentViewCommand = new RelayCommand(Executed_CloseLeaveCommentViewCommand, CanExecute_CloseLeaveCommentViewCommand);
        }
    }
}

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
        public ForumViewModel ForumVM { get; set; }
        
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

        public RelayCommand SubmitCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }


        public LeaveCommentViewModel(LeaveCommentView leaveCommentView, ForumViewModel forumVM, Owner owner, Forum forum)
        {
            InitCommands();

            _forumCommentService = new ForumCommentService();

            LeaveCommentView = leaveCommentView;
            ForumVM = forumVM;
            Owner = owner;
            Forum = forum;
        }

        #region Commands
        public void Executed_SubmitCommand(object obj)
        {          
            ForumComment comment = new ForumComment(Owner, Forum, Comment, true);
            _forumCommentService.Add(comment);
            ForumVM.UpdateComments();
            LeaveCommentView.Close();           
        }

        public void Executed_CloseViewCommand(object obj)
        {
            LeaveCommentView.Close();
        }

        #endregion

        public void InitCommands()
        {
            SubmitCommand = new RelayCommand(Executed_SubmitCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
        }
    }
}

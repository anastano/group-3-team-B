using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class SelectedForumViewModel
    {
        private readonly ForumCommentService _forumCommentService;
        private readonly AccommodationService _accommodationService;

        public SelectedForumView SelectedForumView { get; set; }
        public Owner Owner { get; set; }
        public Forum Forum { get; set; }
        public ObservableCollection<ForumComment> Comments { get; set; }
        public ForumComment SelectedComment { get; set; }
        public RelayCommand LeaveCommentCommand { get; set; }
        public RelayCommand ReportCommentCommand { get; set; }     
        public RelayCommand CloseSelectedForumViewCommand { get; set; }


        public SelectedForumViewModel(SelectedForumView selectedForumView, Owner owner, Forum selectedForum)
        {
            InitCommands();

            _forumCommentService = new ForumCommentService();
            _accommodationService = new AccommodationService();

            SelectedForumView = selectedForumView;
            Owner = owner;
            Forum = selectedForum;
            Comments = new ObservableCollection<ForumComment>(_forumCommentService.GetByForumId(Forum.Id));
        }

        #region Commands
        public void Executed_LeaveCommentCommand(object obj)
        {
            if (_accommodationService.GetByLocationIdAndOwnerId(Forum.LocationId, Owner.Id).Count != 0)
            {
                Window leaveCommentView = new LeaveCommentView(this, Owner, Forum);
                leaveCommentView.ShowDialog();
            }
            else if (Forum.Status.Equals(ForumStatus.CLOSED))
            {
                MessageBox.Show("You are unable to leave a comment as this forum is closed.");
            }
            else
            {
                MessageBox.Show("You are unable to leave a comment as you do not possess any accommodation in this location.");
            }    
        }

        public bool CanExecute_LeaveCommentCommand(object obj)
        {
            return true;
        }

        public void Executed_ReportCommentCommand(object obj)
        {
            if (SelectedComment != null && !SelectedComment.IsUseful)
            {
                if (_forumCommentService.ReportComment(Owner, SelectedComment))
                {
                    UpdateComments();
                }
                else
                {
                    MessageBox.Show("You already have reported this comment");
                }
            }
            else if(SelectedComment == null)
            {
                MessageBox.Show("No comment has been selected");
            }
            else 
            {
                MessageBox.Show("You are unable to report this comment since it is valid.");
            }
        }

        public bool CanExecute_ReportCommentCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseSelectedForumViewCommand(object obj)
        {
            SelectedForumView.Close();
        }

        public bool CanExecute_CloseSelectedForumViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            LeaveCommentCommand = new RelayCommand(Executed_LeaveCommentCommand, CanExecute_LeaveCommentCommand);
            ReportCommentCommand = new RelayCommand(Executed_ReportCommentCommand, CanExecute_ReportCommentCommand);
            CloseSelectedForumViewCommand = new RelayCommand(Executed_CloseSelectedForumViewCommand, CanExecute_CloseSelectedForumViewCommand);
        }

        public void UpdateComments()
        {
            Comments.Clear();
            foreach (ForumComment comment in _forumCommentService.GetByForumId(Forum.Id))
            {
                Comments.Add(comment);
            }
        }
    }
}

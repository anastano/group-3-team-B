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
    public class ForumViewModel
    {
        private readonly ForumCommentService _forumCommentService;
        private readonly AccommodationService _accommodationService;
        public ForumView ForumView { get; set; }
        public Owner Owner { get; set; }
        public Forum Forum { get; set; }
        public ObservableCollection<ForumComment> Comments { get; set; }
        public ForumComment SelectedComment { get; set; }
        public RelayCommand LeaveCommentCommand { get; set; }
        public RelayCommand ReportCommentCommand { get; set; }     
        public RelayCommand CloseViewCommand { get; set; }


        public ForumViewModel(ForumView forumView, Owner owner, Forum forum)
        {
            InitCommands();

            _forumCommentService = new ForumCommentService();
            _accommodationService = new AccommodationService();

            ForumView = forumView;
            Owner = owner;
            Forum = forum;
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

        public void Executed_CloseViewCommand(object obj)
        {
            ForumView.Close();
        }

        #endregion

        public void InitCommands()
        {
            LeaveCommentCommand = new RelayCommand(Executed_LeaveCommentCommand);
            ReportCommentCommand = new RelayCommand(Executed_ReportCommentCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
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

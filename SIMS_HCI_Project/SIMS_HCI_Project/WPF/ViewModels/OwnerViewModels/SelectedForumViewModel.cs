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

            SelectedForumView = selectedForumView;
            Owner = owner;
            Forum = selectedForum;
            Comments = new ObservableCollection<ForumComment>(_forumCommentService.GetByForumId(Forum.Id));
        }

        #region Commands
        public void Executed_LeaveCommentCommand(object obj)
        {
            Window leaveCommentView = new LeaveCommentView(Owner, Forum);
            leaveCommentView.ShowDialog();
        }

        public bool CanExecute_LeaveCommentCommand(object obj)
        {
            return true;
        }

        public void Executed_ReportCommentCommand(object obj)
        {
            if (SelectedComment != null)
            {
                _forumCommentService.ReportComment(SelectedComment);
                UpdateComments();
            }
            else 
            {
                MessageBox.Show("No comment has been selected");
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

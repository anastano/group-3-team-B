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

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class LeaveCommentViewModel
    {
        private readonly ForumCommentService _forumCommentService;
        public LeaveCommentView LeaveCommentView { get; set; }
        public Owner Owner { get; set; }
        public Forum Forum { get; set; }
        public RelayCommand SubmitCommentCommand { get; set; }
        public RelayCommand CloseLeaveCommentViewCommand { get; set; }


        public LeaveCommentViewModel(LeaveCommentView leaveCommentView, Owner owner, Forum forum)
        {
            InitCommands();

            _forumCommentService = new ForumCommentService();

            LeaveCommentView = leaveCommentView;
            Owner = owner;
            Forum = forum;
        }

        #region Commands
        public void Executed_SubmitCommentCommand(object obj)
        {
            //Window selectForumView = new SelectedForumView(this, Owner);
            // selectForumView.ShowDialog();
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

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
    public class ForumsViewModel
    {
        private readonly ForumService _forumService;
        public ForumsView ForumsView { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<Forum> Forums { get; set; }
        public Forum SelectedForum { get; set; }
        public RelayCommand ShowSelectedForumCommand { get; set; }
        public RelayCommand CloseForumsViewCommand { get; set; }


        public ForumsViewModel(ForumsView forumsView, Owner owner)
        {
            InitCommands();

            _forumService = new ForumService();

            ForumsView = forumsView;
            Owner = owner;
            Forums = new ObservableCollection<Forum>(_forumService.GetAll());
        }

        #region Commands
        public void Executed_ShowSelectedForumCommand(object obj)
        {
            if (SelectedForum != null)
            {
                Window selectForumView = new SelectedForumView(Owner, SelectedForum);
                selectForumView.ShowDialog();
            }
            else
            {
                MessageBox.Show("No forum has been selected");
            }
        }

        public bool CanExecute_ShowSelectedForumCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseForumsViewCommand(object obj)
        {
            ForumsView.Close();
        }

        public bool CanExecute_CloseForumsViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            ShowSelectedForumCommand = new RelayCommand(Executed_ShowSelectedForumCommand, CanExecute_ShowSelectedForumCommand);
            CloseForumsViewCommand = new RelayCommand(Executed_CloseForumsViewCommand, CanExecute_CloseForumsViewCommand);
        }
    }
}

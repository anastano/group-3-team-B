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
        public RelayCommand ShowForumCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }


        public ForumsViewModel(ForumsView forumsView, Owner owner)
        {
            InitCommands();

            _forumService = new ForumService();

            ForumsView = forumsView;
            Owner = owner;
            Forums = new ObservableCollection<Forum>(_forumService.GetAll());
        }

        #region Commands
        public void Executed_ShowForumCommand(object obj)
        {
            if (SelectedForum != null)
            {
                Window selectForumView = new ForumView(Owner, SelectedForum);
                selectForumView.ShowDialog();
            }
            else
            {
                MessageBox.Show("No forum has been selected");
            }
        }

        public void Executed_CloseViewCommand(object obj)
        {
            ForumsView.Close();
        }

        #endregion

        public void InitCommands()
        {
            ShowForumCommand = new RelayCommand(Executed_ShowForumCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
        }
    }
}

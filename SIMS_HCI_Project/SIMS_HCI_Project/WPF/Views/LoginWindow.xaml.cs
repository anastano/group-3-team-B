using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIMS_HCI_Project.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        private readonly UserService _userService;

        public ObservableCollection<User> Users { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _userService = new UserService();

            Users = new ObservableCollection<User>(_userService.GetAll());

        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {

            lblErrorMessage.Content = "";
            Window win = null;
            User user = _userService.LogIn(Username, Password);

            if (user != null)
            {
                switch (user.Role)
                {
                    case UserRole.OWNER:
                        Window ownerView = new OwnerMainView(new Owner(user));
                        ownerView.Show();
                        break;
                    case UserRole.GUEST1:
                        win = null;
                        //win.Show();

                        break;
                    case UserRole.GUEST2:
                        // Window guest2View = new Guest2View(new Guest2(user.Id, user.Username, user.Password, user.Role));
                        Window guest2View = null;
                        //guest2View.Show();
                        break;
                    case UserRole.GUIDE:
                        Window guideWindow = null;
                        //guideWindow.Show();
                        break;
                }
            }
            else
            {
                lblErrorMessage.Content = "Inccorect username or password";
            }
        }
    }
}

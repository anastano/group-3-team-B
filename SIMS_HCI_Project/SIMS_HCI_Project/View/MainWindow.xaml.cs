using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIMS_HCI_Project
{
    public partial class MainWindow : Window
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        private readonly UserController _userController;

        public MainWindow()
        {
            InitializeComponent();
            _userController = new UserController();
            DataContext = this;
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            lblErrorMessage.Content = "";
            Window win = null;
            User user = _userController.LogIn(Username, Password);
            if (user != null)
            {
                switch (user.Role)
                {
                    case UserRole.OWNER:
                        Window ownerView = new OwnerView(new Owner(user));
                        ownerView.Show();
                        break;
                    case UserRole.GUEST1:
                        win = new Guest1View(new Guest1(user.Id, user.Username, user.Password));
                        win.Show();

                        break;
                    case UserRole.GUEST2:
                        Window guest2View = new Guest2View(new Guest2(user.Id, user.Username, user.Password));
                        guest2View.Show();
                        break;
                    case UserRole.GUIDE:
                        Window guideWindow = new GuideMainView(new Guide(user));
                        guideWindow.Show();
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

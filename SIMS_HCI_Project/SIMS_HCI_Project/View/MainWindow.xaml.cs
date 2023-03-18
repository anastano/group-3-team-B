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
                switch (user.Id[0])
                {
                    case 'O':
                        //TODO: Forward to owner view
                        Window ownerView = new OwnerView(user.Id);
                        ownerView.Show();
                        break;
                    case 'F':
                        //TODO: Forward to guest1 view
                        win = new Gues1View();
                        win.Show();

                        break;
                    case 'S':
                        Window guest2View = new Guest2View();
                        guest2View.Show();
                        break;
                    case 'G':
                        Window guideWindow = new GuideMainView(new Guide(user.Id, user.Username, user.Password));
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

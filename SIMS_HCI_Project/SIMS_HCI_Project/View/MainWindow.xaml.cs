using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Repository;
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

        private readonly UserRepository _repository;

        public MainWindow()
        {
            InitializeComponent();
            _repository = new UserRepository();

            DataContext = this;
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            lblErrorMessage.Content = "";

            User user = _repository.GetByUsernameAndPassword(Username, Password);
            if (user != null)
            {
                switch (user.Username[0])
                {
                    case 'O':
                        //TODO: Forward to owner view
                        break;
                    case 'F':
                        //TODO: Forward to guest1 view
                        break;
                    case 'S':
                        //TODO: Forward to guest2 view
                        break;
                    case 'G':
                        //TODO: Forward to guide view
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

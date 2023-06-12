using SIMS_HCI_Project.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace SIMS_HCI_Project.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {   
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel(this);            
        }

    }
}

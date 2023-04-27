﻿using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views;
using SIMS_HCI_Project.WPF.Views.GuideViews;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels
{
    public class LoginViewModel
    {
        public string? Username { get; set; }
        public LoginWindow LoginWindow { get; set;}

        public RelayCommand LoginCommand { get; set; }

        private readonly UserService _userService;
        private readonly StartupService _startupService;

        public LoginViewModel(LoginWindow loginWindow)
        {
            _userService = new UserService();
            _startupService = new StartupService();
            LoginWindow = loginWindow;

            LoginCommand = new RelayCommand(Executed_LoginCommand, CanExecute_LoginCommand);
        }

        public void Executed_LoginCommand(object obj)
        {
            LoginWindow.lblErrorMessage.Content = "";
            User user = _userService.GetByUsernameAndPassword(Username, LoginWindow.pbPassword.Password);

            if (user != null)
            {
                _startupService.LoadConnections();
                switch (user.Role)
                {
                    case UserRole.OWNER:
                        Window ownerView = new OwnerMainView(new Owner(user));
                        ownerView.Show();
                        break;
                    case UserRole.GUEST1:
                        Window guest1View = new Guest1MainView(new Guest1(user));
                        guest1View.Show();
                        //win.Show();

                        break;
                    case UserRole.GUEST2:
                        // Window guest2View = new Guest2View(new Guest2(user.Id, user.Username, user.Password, user.Role));
                        //Window guest2View = null;
                        //guest2View.Show();
                        Window guest2View = new Guest2View(new Guest2(user));
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
                LoginWindow.lblErrorMessage.Content = "Incorrect username or password";
            }
        }

        public bool CanExecute_LoginCommand(object obj)
        {
            return true;
        }
        
    }
}

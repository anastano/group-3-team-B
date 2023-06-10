﻿using SIMS_HCI_Project.WPF.ViewModels.GuideViewModels;
using System;
using System.Collections.Generic;
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

namespace SIMS_HCI_Project.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for ComplexTourRequestsView.xaml
    /// </summary>
    public partial class ComplexTourRequestsView : Window
    {
        public ComplexTourRequestsView()
        {
            InitializeComponent();

            this.DataContext = new ComplexTourRequestsViewModel();
        }
    }
}

using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public User? CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Set the culture for the entire application
            CultureInfo culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            //CurrentUser = null;

            App.Current.Properties["CurrentUser"] = null;

            base.OnStartup(e);
        }
    }
}

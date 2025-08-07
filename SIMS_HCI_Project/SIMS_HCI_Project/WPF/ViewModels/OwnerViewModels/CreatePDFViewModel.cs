using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class CreatePDFViewModel : INotifyPropertyChanged
    {
        public CreatePDFView CreatePDFView{ get; set; }
        public Owner Owner { get; set; }

        #region OnPropertyChanged

        private DateTime _enteredStart;
        public DateTime EnteredStart
        {
            get => _enteredStart;
            set
            {
                if (value != _enteredStart)
                {
                    _enteredStart = value;
                    OnPropertyChanged(nameof(EnteredStart));
                }
            }
        }

        private DateTime _enteredEnd;
        public DateTime EnteredEnd
        {
            get => _enteredEnd;
            set
            {
                if (value != _enteredEnd)
                {
                    _enteredEnd = value;
                    OnPropertyChanged(nameof(EnteredEnd));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public RelayCommand ShowPDFCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }

        public CreatePDFViewModel(CreatePDFView createPDFView, Owner owner) 
        {
            InitCommands();

            CreatePDFView = createPDFView;
            Owner = owner;

            EnteredStart = DateTime.Today.AddDays(1);
            EnteredEnd = DateTime.Today.AddDays(1);
        }

        #region Commands
        public void Executed_ShowPDFCommand(object obj)
        {
            DateRange dateRange = new DateRange(EnteredStart, EnteredEnd);
            Window pdfReportView = new PDFReportView(CreatePDFView, Owner, dateRange);
            pdfReportView.ShowDialog();
        }

        public void Executed_CloseViewCommand(object obj)
        {
            CreatePDFView.Close();
        }

        #endregion

        public void InitCommands()
        {
            ShowPDFCommand = new RelayCommand(Executed_ShowPDFCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
        }
    }
}

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
        private RenovationService _renovationService;
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

        #endregion

        public RelayCommand ShowPDFCommand { get; set; }
        public RelayCommand CloseCreatePDFViewCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreatePDFViewModel(CreatePDFView createPDFView, RenovationService renovationService, Owner owner) 
        {
            InitCommands();

            _renovationService = renovationService;

            CreatePDFView = createPDFView;
            Owner = owner;

            EnteredStart = DateTime.Today.AddDays(1);
            EnteredEnd = DateTime.Today.AddDays(1);
        }

        #region Commands
        public void Executed_ShowPDFCommand(object obj)
        {
            DateRange dateRange = new DateRange(EnteredStart, EnteredEnd);
            Window pdfReportView = new PDFReportView(CreatePDFView, _renovationService, Owner, dateRange);
            pdfReportView.ShowDialog();
        }

        public bool CanExecute_ShowPDFCommand(object obj)
        {
            return true;
        }
        public void Executed_CloseCreatePDFViewCommand(object obj)
        {
            CreatePDFView.Close();
        }

        public bool CanExecute_CloseCreatePDFViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            ShowPDFCommand = new RelayCommand(Executed_ShowPDFCommand, CanExecute_ShowPDFCommand);
            CloseCreatePDFViewCommand = new RelayCommand(Executed_CloseCreatePDFViewCommand, CanExecute_CloseCreatePDFViewCommand);
        }
    }
}

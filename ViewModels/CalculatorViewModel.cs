using lab3.Models;
using lab3.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace lab3.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private readonly CalculatorModel _model;
        private readonly HistoryService _historyService;
        private string _display = "0";

        public CalculatorViewModel()
        {
            _model = new CalculatorModel();
            _historyService = new HistoryService();
            _model.OnDisplayChanged += text => Display = text;

            NumberCommand = new RelayCommand(InputNumber);
            OperatorCommand = new RelayCommand(SetOperator);
            EqualsCommand = new RelayCommand(Calculate);
            ClearCommand = new RelayCommand(Clear);
            NegateCommand = new RelayCommand(Negate);
            PercentCommand = new RelayCommand(Percent);
            ClearHistoryCommand = new RelayCommand(ClearHistory);
        }

        public string Display
        {
            get { return _display; }
            set
            {
                _display = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<HistoryItem> History
        {
            get { return _historyService.History; }
        }

        public ICommand NumberCommand { get; private set; }
        public ICommand OperatorCommand { get; private set; }
        public ICommand EqualsCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand NegateCommand { get; private set; }
        public ICommand PercentCommand { get; private set; }
        public ICommand ClearHistoryCommand { get; private set; }

        private void InputNumber(object parameter)
        {
            _model.InputNumber(parameter?.ToString());
        }

        private void SetOperator(object parameter)
        {
            _model.SetOperator(parameter?.ToString());
        }

        private void Calculate(object parameter)
        {
            _model.Calculate();
            OnPropertyChanged(nameof(History));
        }

        private void Clear(object parameter)
        {
            _model.Clear();
        }

        private void Negate(object parameter)
        {
            _model.Negate();
        }

        private void Percent(object parameter)
        {
            _model.Percent();
        }

        private void ClearHistory(object parameter)
        {
            _historyService.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
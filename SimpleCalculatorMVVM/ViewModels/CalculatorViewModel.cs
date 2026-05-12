#nullable disable

using Lab6_Resources.Models;
using Lab6_Resources.Services;
using Lab6_Resources.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Lab6_Resources.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private readonly CalculatorModel _model;
        private readonly HistoryService _historyService;
        private string _displayText = "0";
        private string _operationIndicator = "";

        public CalculatorViewModel()
        {
            _model = new CalculatorModel();
            _historyService = new HistoryService();
            _model.OnDisplayChanged += text => DisplayText = text;
            InitializeCommands();
        }

        public string DisplayText
        {
            get => _displayText;
            set { _displayText = value; OnPropertyChanged(nameof(DisplayText)); }
        }

        public string OperationIndicator
        {
            get => _operationIndicator;
            set { _operationIndicator = value; OnPropertyChanged(nameof(OperationIndicator)); }
        }

        public ObservableCollection<HistoryItem> History => _historyService.History;

        // Команды
        public ICommand Input1Command { get; private set; }
        public ICommand Input2Command { get; private set; }
        public ICommand Input3Command { get; private set; }
        public ICommand Input4Command { get; private set; }
        public ICommand Input5Command { get; private set; }
        public ICommand Input6Command { get; private set; }
        public ICommand Input7Command { get; private set; }
        public ICommand Input8Command { get; private set; }
        public ICommand Input9Command { get; private set; }
        public ICommand Input0Command { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand SubtractCommand { get; private set; }
        public ICommand MultiplyCommand { get; private set; }
        public ICommand DivideCommand { get; private set; }
        public ICommand EqualsCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand ClearEntryCommand { get; private set; }
        public ICommand DecimalCommand { get; private set; }
        public ICommand ToggleSignCommand { get; private set; }
        public ICommand PercentageCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand ClearHistoryCommand { get; private set; }

        private void InitializeCommands()
        {
            Input1Command = new RelayCommand(_ => InputDigit("1"));
            Input2Command = new RelayCommand(_ => InputDigit("2"));
            Input3Command = new RelayCommand(_ => InputDigit("3"));
            Input4Command = new RelayCommand(_ => InputDigit("4"));
            Input5Command = new RelayCommand(_ => InputDigit("5"));
            Input6Command = new RelayCommand(_ => InputDigit("6"));
            Input7Command = new RelayCommand(_ => InputDigit("7"));
            Input8Command = new RelayCommand(_ => InputDigit("8"));
            Input9Command = new RelayCommand(_ => InputDigit("9"));
            Input0Command = new RelayCommand(_ => InputDigit("0"));

            AddCommand = new RelayCommand(_ => SetOperation("+"));
            SubtractCommand = new RelayCommand(_ => SetOperation("-"));
            MultiplyCommand = new RelayCommand(_ => SetOperation("*"));
            DivideCommand = new RelayCommand(_ => SetOperation("/"));
            EqualsCommand = new RelayCommand(_ => Calculate());

            ClearCommand = new RelayCommand(_ => Clear());
            ClearEntryCommand = new RelayCommand(_ => ClearEntry());
            DecimalCommand = new RelayCommand(_ => InputDecimal());
            ToggleSignCommand = new RelayCommand(_ => ToggleSign());
            PercentageCommand = new RelayCommand(_ => Percentage());

            UndoCommand = new RelayCommand(_ => { });
            RedoCommand = new RelayCommand(_ => { });
            ClearHistoryCommand = new RelayCommand(_ => _historyService.Clear());
        }

        private void InputDigit(string digit) => _model.InputDigit(digit);
        private void InputDecimal() => _model.InputDecimal();
        private void SetOperation(string op) { _model.SetOperation(op); OperationIndicator = op; }
        private void Calculate() { _model.Calculate(); OperationIndicator = ""; }
        private void Clear() => _model.Clear();
        private void ClearEntry() => _model.ClearEntry();
        private void ToggleSign() => _model.ToggleSign();
        private void Percentage() => _model.Percentage();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
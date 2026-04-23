using System;

namespace lab3.Models
{
    public class CalculatorModel
    {
        private double _firstNumber = 0;
        private string _currentOperator = "";
        private bool _isNewEntry = true;

        public event Action<string> OnDisplayChanged;
        public string CurrentInput { get; private set; } = "0";

        public void InputNumber(string value)
        {
            if (_isNewEntry)
            {
                CurrentInput = value == "," ? "0," : value;
                _isNewEntry = false;
            }
            else
            {
                if (value == "," && CurrentInput.Contains(",")) return;
                if (CurrentInput == "0" && value != ",") CurrentInput = value;
                else CurrentInput += value;
            }
            OnDisplayChanged?.Invoke(CurrentInput);
        }

        public void SetOperator(string op)
        {
            if (!string.IsNullOrEmpty(_currentOperator) && !_isNewEntry)
                Calculate();
            else
                _firstNumber = double.Parse(CurrentInput);

            _currentOperator = op;
            _isNewEntry = true;
        }

        public double Calculate()
        {
            double secondNumber = double.Parse(CurrentInput);
            double result = _currentOperator switch
            {
                "+" => _firstNumber + secondNumber,
                "-" => _firstNumber - secondNumber,
                "*" => _firstNumber * secondNumber,
                "/" => secondNumber != 0 ? _firstNumber / secondNumber : 0,
                _ => 0
            };

            CurrentInput = result.ToString("G15");
            _firstNumber = result;
            _isNewEntry = true;
            OnDisplayChanged?.Invoke(CurrentInput);
            return result;
        }

        public void Clear()
        {
            _firstNumber = 0;
            _currentOperator = "";
            CurrentInput = "0";
            _isNewEntry = true;
            OnDisplayChanged?.Invoke(CurrentInput);
        }

        public void Negate()
        {
            if (double.TryParse(CurrentInput, out double val))
            {
                CurrentInput = (-val).ToString("G15");
                OnDisplayChanged?.Invoke(CurrentInput);
            }
        }

        public void Percent()
        {
            if (double.TryParse(CurrentInput, out double val))
            {
                CurrentInput = (val / 100).ToString("G15");
                OnDisplayChanged?.Invoke(CurrentInput);
            }
        }
    }
}
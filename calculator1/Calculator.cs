using System;
using System.Globalization;
using System.Linq;

namespace calculator1
{
    public class Calculator
    {
        private string _currentInput = "";
        private string _operation = "";
        private double _firstNumber = 0;
        private bool _isOperationPressed = false;

        public string CurrentInput => _currentInput;
        public string Operation => _operation;

        public void SetCurrentInput(string value)
        {
            _currentInput = value ?? "";
        }

        public string Display =>
            string.IsNullOrEmpty(_currentInput)
                ? ""
                : _operation == ""
                    ? _currentInput
                    : $"{_currentInput} {_operation}";

        public void AppendDigit(char digit)
        {
            if (_isOperationPressed)
            {
                _currentInput = "";
                _isOperationPressed = false;
            }

            // Не добавляем точку, если она уже есть
            if (digit == '.' && _currentInput.Contains('.'))
                return;

            // Проверяем, что минус можно ставить только в начале
            if (digit == '-' && _currentInput.Length > 0)
                return;

            _currentInput += digit;
        }

        // Метод для переключения знака числа
        public void ToggleSign()
        {
            if (string.IsNullOrEmpty(_currentInput)) return;

            if (_currentInput.StartsWith("-"))
                _currentInput = _currentInput.Substring(1);
            else
                _currentInput = "-" + _currentInput;
        }

        public void SetOperation(string op)
        {
            if (string.IsNullOrEmpty(_currentInput)) return;

            if (!string.IsNullOrEmpty(_operation) && !_isOperationPressed)
            {
                Calculate();
            }

            // Парсим первое число с поддержкой минуса
            _firstNumber = double.Parse(_currentInput, CultureInfo.InvariantCulture);
            _operation = op;
            _isOperationPressed = true;
        }

        public double? Calculate()
        {
            if (string.IsNullOrEmpty(_currentInput) || string.IsNullOrEmpty(_operation))
                return null;

            double secondNumber;
            try
            {
                secondNumber = double.Parse(_currentInput, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }

            double result;
            switch (_operation)
            {
                case "+": result = _firstNumber + secondNumber; break;
                case "-": result = _firstNumber - secondNumber; break;
                case "*": result = _firstNumber * secondNumber; break;
                case "/":
                    if (Math.Abs(secondNumber) < double.Epsilon) return null;
                    result = _firstNumber / secondNumber;
                    break;
                default: return null;
            }

            _currentInput = result.ToString(CultureInfo.InvariantCulture);
            _operation = "";
            _isOperationPressed = true;
            return result;
        }

        public void Clear()
        {
            _currentInput = "";
            _operation = "";
            _firstNumber = 0;
            _isOperationPressed = false;
        }

        public double GetCurrentNumber()
            => double.Parse(_currentInput ?? "0", CultureInfo.InvariantCulture);

        public bool HasPendingOperation => !string.IsNullOrEmpty(_operation);
    }
}
#nullable disable

using System;

namespace Lab6_Resources.Models
{
    public class CalculatorModel
    {
        public event Action<string> OnDisplayChanged;

        private double _currentValue = 0;
        private double _storedValue = 0;
        private string _pendingOperation = null;
        private bool _isNewEntry = true;

        public string DisplayValue { get; private set; } = "0";

        public void InputDigit(string digit)
        {
            if (_isNewEntry)
            {
                _currentValue = double.Parse(digit);
                _isNewEntry = false;
            }
            else
            {
                _currentValue = double.Parse(DisplayValue + digit);
            }
            DisplayValue = _currentValue.ToString("G15");
            OnDisplayChanged?.Invoke(DisplayValue);
        }

        public void InputDecimal()
        {
            if (_isNewEntry)
            {
                _currentValue = 0;
                _isNewEntry = false;
            }
            if (!DisplayValue.Contains(","))
            {
                DisplayValue += ",";
                OnDisplayChanged?.Invoke(DisplayValue);
            }
        }

        public void SetOperation(string operation)
        {
            if (_pendingOperation != null && !_isNewEntry)
            {
                Calculate();
            }
            _storedValue = _currentValue;
            _pendingOperation = operation;
            _isNewEntry = true;
        }

        public void Calculate()
        {
            if (_pendingOperation == null) return;

            double result = _pendingOperation switch
            {
                "+" => _storedValue + _currentValue,
                "-" => _storedValue - _currentValue,
                "*" => _storedValue * _currentValue,
                "/" => _currentValue != 0 ? _storedValue / _currentValue : 0,
                _ => _currentValue
            };

            _currentValue = result;
            _storedValue = result;
            DisplayValue = result.ToString("G15");
            _pendingOperation = null;
            _isNewEntry = true;
            OnDisplayChanged?.Invoke(DisplayValue);
        }

        public void Clear()
        {
            _currentValue = 0;
            _storedValue = 0;
            _pendingOperation = null;
            DisplayValue = "0";
            _isNewEntry = true;
            OnDisplayChanged?.Invoke(DisplayValue);
        }

        public void ClearEntry()
        {
            _currentValue = 0;
            DisplayValue = "0";
            _isNewEntry = true;
            OnDisplayChanged?.Invoke(DisplayValue);
        }

        public void ToggleSign()
        {
            _currentValue = -_currentValue;
            DisplayValue = _currentValue.ToString("G15");
            OnDisplayChanged?.Invoke(DisplayValue);
        }

        public void Percentage()
        {
            _currentValue = _currentValue / 100;
            DisplayValue = _currentValue.ToString("G15");
            OnDisplayChanged?.Invoke(DisplayValue);
        }
    }
}
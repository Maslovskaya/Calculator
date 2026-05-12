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
        private bool _isDummyMode = false;

        public string DisplayValue { get; private set; } = "0";

        public void SetDummyMode(bool enabled)
        {
            _isDummyMode = enabled;
        }

        public bool IsDummyMode => _isDummyMode;

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

            if (_isDummyMode)
            {
                // 🤓 РЕЖИМ ДЛЯ ЧАЙНИКОВ - ПРИКОЛЫ!
                CalculateDummy();
            }
            else
            {
                // ✅ Обычный режим калькулятора
                CalculateNormal();
            }

            _pendingOperation = null;
            _isNewEntry = true;
            OnDisplayChanged?.Invoke(DisplayValue);
        }

        private void CalculateDummy()
        {
            string num1 = _storedValue.ToString("G15").Replace(",", "");
            string num2 = _currentValue.ToString("G15").Replace(",", "");

            switch (_pendingOperation)
            {
                case "+":
                    // Сложение: склеиваем числа (2+6=26)
                    DisplayValue = num1 + num2;
                    _currentValue = double.Parse(DisplayValue);
                    _storedValue = _currentValue;
                    break;

                case "-":
                    // Вычитание: обратное склеивание (9-2=29)
                    DisplayValue = num2 + num1;
                    _currentValue = double.Parse(DisplayValue);
                    _storedValue = _currentValue;
                    break;

                case "*":
                    // Умножение: первое число повторяется (2*3=222)
                    int repeatCount = (int)Math.Abs(_currentValue);
                    if (repeatCount == 0) repeatCount = 1;
                    DisplayValue = "";
                    for (int i = 0; i < repeatCount; i++)
                    {
                        DisplayValue += num1;
                    }
                    _currentValue = double.Parse(DisplayValue);
                    _storedValue = _currentValue;
                    break;

                case "/":
                    // Деление: смешная ошибка
                    if (num2 == "0")
                    {
                        DisplayValue = "🤔 НА НОЛЬ?";
                    }
                    else
                    {
                        // Показываем первое число с вопросиком
                        DisplayValue = num1 + "?";
                    }
                    _currentValue = 0;
                    _storedValue = 0;
                    break;

                default:
                    DisplayValue = num1;
                    _currentValue = double.Parse(DisplayValue);
                    _storedValue = _currentValue;
                    break;
            }
        }

        private void CalculateNormal()
        {
            double result = _pendingOperation switch
            {
                "+" => _storedValue + _currentValue,
                "-" => _storedValue - _currentValue,
                "*" => _storedValue * _currentValue,
                "/" => _currentValue != 0 ? _storedValue / _currentValue : 0,
                _ => _currentValue
            };

            DisplayValue = result.ToString("G15");
            _currentValue = result;
            _storedValue = result;
        }

        public void Clear()
        {
            _currentValue = 0;
            _storedValue = 0;
            _pendingOperation = null;
            DisplayValue = "0";
            _isNewEntry = true;
            _isDummyMode = false; // Сбрасываем режим чайников
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
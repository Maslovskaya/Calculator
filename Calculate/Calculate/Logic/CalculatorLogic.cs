/*
 * CalculatorLogic.cs - Бизнес-логика калькулятора
 * 
 * Это "мозг" приложения. Здесь происходят все вычисления.
 * Класс не знает ничего об интерфейсе (WPF) - только чистая логика.
 * 
 * Используемые паттерны:
 * 1. Observer: события DisplayUpdated и CalculationPerformed
 * 2. Strategy: использует IOperation для вычислений
 * 3. Dependency Injection: принимает зависимости через конструктор
 * 
 * Преимущества:
 * - Тестируемость: можно тестировать без запуска UI
 * - Независимость: можно заменить WPF на консоль или веб
 * - Расширяемость: легко добавить новые функции
 */

using Calculator.Managers;
using System;

namespace Calculator.Logic
{
    public class CalculatorLogic
    {
        // === Состояние калькулятора ===
        private double _firstNumber = 0;        // Первое число в операции
        private double _secondNumber = 0;       // Второе число в операции
        private IOperation _currentOperation;   // Текущая операция (Strategy)
        private string _currentInput = "0";     // Текущий ввод на экране
        private bool _isNewEntry = true;        // Флаг: начало нового ввода

        // === События для UI (Observer Pattern) ===
        // Когда нужно обновить дисплей
        public event Action<string> DisplayUpdated;

        // Когда вычисление завершено (для истории)
        public event Action<string, string> CalculationPerformed;

        // === Ввод числа ===
        public void InputNumber(string value)
        {
            // Если начался новый ввод (после операции или =), очищаем экран
            if (_isNewEntry)
            {
                _currentInput = value == "," ? "0," : value;
                _isNewEntry = false;
            }
            else
            {
                // Защита от нескольких запятых
                if (value == "," && _currentInput.Contains(",")) return;

                // Замена ведущего нуля
                if (_currentInput == "0" && value != ",") _currentInput = value;
                else _currentInput += value;
            }

            // Уведомляем UI об изменении (Observer)
            DisplayUpdated?.Invoke(_currentInput);
        }

        // === Установка оператора ===
        public void SetOperator(string op)
        {
            // Если уже есть операция и введено второе число - считаем промежуточный результат
            if (_currentOperation != null && !_isNewEntry)
                Calculate();
            else
                _firstNumber = double.Parse(_currentInput);

            // Получаем стратегию операции через фабрику
            _currentOperation = OperationFactory.GetOperation(op);
            _isNewEntry = true;  // Готовимся к вводу второго числа
        }

        // === Выполнение вычисления ===
        public void Calculate()
        {
            if (_currentOperation == null) return;

            _secondNumber = double.Parse(_currentInput);
            string expression = $"{_firstNumber} {_currentOperation.Symbol} {_secondNumber}";

            try
            {
                // Выполняем операцию через стратегию (Strategy Pattern)
                double result = _currentOperation.Execute(_firstNumber, _secondNumber);
                _currentInput = result.ToString("G15");  // G15 - точность до 15 знаков
                _firstNumber = result;  // Результат становится первым числом для цепочки

                // Добавляем в историю (Singleton)
                HistoryManager.Instance.AddEntry(expression, _currentInput);

                // Уведомляем UI о завершении вычисления
                CalculationPerformed?.Invoke(expression, _currentInput);
            }
            catch (DivideByZeroException)
            {
                _currentInput = "Ошибка";
            }
            catch (Exception)
            {
                _currentInput = "Ошибка";
            }

            _isNewEntry = true;
            DisplayUpdated?.Invoke(_currentInput);
        }

        // === Очистка калькулятора ===
        public void Clear()
        {
            _firstNumber = 0;
            _secondNumber = 0;
            _currentOperation = null;
            _currentInput = "0";
            _isNewEntry = true;
            DisplayUpdated?.Invoke(_currentInput);
        }

        // === Смена знака (±) ===
        public void Negate()
        {
            if (double.TryParse(_currentInput, out double val))
            {
                _currentInput = (-val).ToString("G15");
                DisplayUpdated?.Invoke(_currentInput);
            }
        }

        // === Проценты (%) ===
        public void Percent()
        {
            if (double.TryParse(_currentInput, out double val))
            {
                _currentInput = (val / 100).ToString("G15");
                DisplayUpdated?.Invoke(_currentInput);
            }
        }
    }
}
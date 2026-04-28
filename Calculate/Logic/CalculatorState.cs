/*
 * CalculatorState.cs - Состояние калькулятора для поддержки отмены операций
 * 
 * Паттерн: Memento (Хранитель) - используется в связке с Command
 * 
 * Назначение:
 * - Хранит текущее состояние калькулятора для возможности отката
 * - Инкапсулирует данные, необходимые для восстановления состояния
 */

namespace Calculator.Logic
{
    public class CalculatorState
    {
        public double FirstNumber { get; set; }
        public double SecondNumber { get; set; }
        public IOperation CurrentOperation { get; set; }
        public string CurrentInput { get; set; }
        public bool IsNewEntry { get; set; }
    }
}
/*
 * NumberCommand.cs - Команда ввода цифры (реализация Command Pattern)
 * 
 * Назначение:
 * - Инкапсулирует операцию ввода цифры
 * - Позволяет отменить ввод последней цифры
 */

using Calculator.Logic;
using Calculator.Commands;

namespace Calculator.Commands
{
    public class NumberCommand : ICommand
    {
        private readonly CalculatorLogic _logic;
        private readonly string _number;
        private CalculatorState _previousState;

        public NumberCommand(CalculatorLogic logic, string number)
        {
            _logic = logic;
            _number = number;
        }

        public void Execute()
        {
            // Сохраняем состояние перед выполнением
            _previousState = _logic.SaveState();
            _logic.InputNumber(_number);
        }

        public void Undo()
        {
            // Восстанавливаем состояние
            _logic.RestoreState(_previousState);
        }
    }
}
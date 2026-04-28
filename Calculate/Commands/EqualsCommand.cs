/*
 * EqualsCommand.cs - Команда вычисления результата (реализация Command Pattern)
 * 
 * Назначение:
 * - Инкапсулирует операцию вычисления
 * - Позволяет отменить последнее вычисление
 */

using Calculator.Logic;
using Calculator.Commands;

namespace Calculator.Commands
{
    public class EqualsCommand : ICommand
    {
        private readonly CalculatorLogic _logic;
        private CalculatorState _previousState;

        public EqualsCommand(CalculatorLogic logic)
        {
            _logic = logic;
        }

        public void Execute()
        {
            _previousState = _logic.SaveState();
            _logic.Calculate();
        }

        public void Undo()
        {
            _logic.RestoreState(_previousState);
        }
    }
}
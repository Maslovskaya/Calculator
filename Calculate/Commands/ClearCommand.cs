/*
 * ClearCommand.cs - Команда очистки (реализация Command Pattern)
 * 
 * Назначение:
 * - Инкапсулирует операцию очистки
 * - Позволяет отменить очистку (вернуться к предыдущему состоянию)
 */

using Calculator.Logic;
using Calculator.Commands;

namespace Calculator.Commands
{
    public class ClearCommand : ICommand
    {
        private readonly CalculatorLogic _logic;
        private CalculatorState _previousState;

        public ClearCommand(CalculatorLogic logic)
        {
            _logic = logic;
        }

        public void Execute()
        {
            _previousState = _logic.SaveState();
            _logic.Clear();
        }

        public void Undo()
        {
            _logic.RestoreState(_previousState);
        }
    }
}
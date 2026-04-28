/*
 * OperatorCommand.cs - Команда установки оператора (реализация Command Pattern)
 * 
 * Назначение:
 * - Инкапсулирует операцию выбора математического оператора
 * - Позволяет отменить выбор оператора
 */

using Calculator.Logic;
using Calculator.Commands;

namespace Calculator.Commands
{
    public class OperatorCommand : ICommand
    {
        private readonly CalculatorLogic _logic;
        private readonly string _operator;
        private CalculatorState _previousState;

        public OperatorCommand(CalculatorLogic logic, string op)
        {
            _logic = logic;
            _operator = op;
        }

        public void Execute()
        {
            _previousState = _logic.SaveState();
            _logic.SetOperator(_operator);
        }

        public void Undo()
        {
            _logic.RestoreState(_previousState);
        }
    }
}
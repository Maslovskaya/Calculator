/*
 * CommandManager.cs - Менеджер команд (реализация Command Pattern)
 * 
 * Назначение:
 * - Управляет историей выполненных команд
 * - Обеспечивает поддержку отмены и повтора операций
 * 
 * Преимущества:
 * - Централизованное управление операциями
 * - Легкая реализация Undo/Redo
 * - Отделение логики управления от бизнес-логики
 */

using System.Collections.Generic;
using Calculator.Commands;

namespace Calculator.Commands
{
    public class CommandManager
    {
        private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear(); // После новой команды нельзя повторять отмененные
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                ICommand command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                ICommand command = _redoStack.Pop();
                command.Execute();
                _undoStack.Push(command);
            }
        }
    }
}
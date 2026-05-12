/*
 * LoggingOperationDecorator.cs - Добавляет логирование операций
 * Пример структурного паттерна Decorator
 */
#nullable disable
using System;
using System.IO;

namespace Lab6_Resources.Decorators
{
    public class LoggingOperationDecorator : IOperationDecorator
    {
        private readonly IOperationDecorator _decorated;
        private readonly string _logFile = "calculator_log.txt";

        public LoggingOperationDecorator(IOperationDecorator decorated)
        {
            _decorated = decorated;
        }

        public string Name => $"{_decorated.Name} (с логированием)";

        public double Execute(double a, double b)
        {
            // Логирование ДО выполнения
            Log($"Начало: {_decorated.Name}({a}, {b})");

            try
            {
                var result = _decorated.Execute(a, b);
                // Логирование ПОСЛЕ выполнения
                Log($"Результат: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Log($"Ошибка: {ex.Message}");
                throw;
            }
        }

        private void Log(string message)
        {
            var logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";
            File.AppendAllText(_logFile, logEntry + Environment.NewLine);
        }
    }
}
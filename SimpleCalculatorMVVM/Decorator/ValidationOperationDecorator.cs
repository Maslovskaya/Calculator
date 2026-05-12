/*
 * ValidationOperationDecorator.cs - Добавляет валидацию операций
 */
#nullable disable
using System;

namespace Lab6_Resources.Decorators
{
    public class ValidationOperationDecorator : IOperationDecorator
    {
        private readonly IOperationDecorator _decorated;

        public ValidationOperationDecorator(IOperationDecorator decorated)
        {
            _decorated = decorated;
        }

        public string Name => $"{_decorated.Name} (с валидацией)";

        public double Execute(double a, double b)
        {
            // Валидация перед выполнением
            if (double.IsNaN(a) || double.IsNaN(b))
                throw new ArgumentException("Недопустимые числа");

            if (double.IsInfinity(a) || double.IsInfinity(b))
                throw new ArgumentException("Бесконечные значения не допускаются");

            return _decorated.Execute(a, b);
        }
    }
}
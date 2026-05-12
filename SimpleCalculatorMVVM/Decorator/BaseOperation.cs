/*
 * BaseOperation.cs - Базовый класс операции
 */
#nullable disable
namespace Lab6_Resources.Decorators
{
    public class BaseOperation : IOperationDecorator
    {
        private readonly string _name;
        private readonly Func<double, double, double> _operation;

        public BaseOperation(string name, Func<double, double, double> operation)
        {
            _name = name;
            _operation = operation;
        }

        public string Name => _name;
        public double Execute(double a, double b) => _operation(a, b);
    }
}
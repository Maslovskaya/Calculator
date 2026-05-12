/*
 * IOperationDecorator.cs - Интерфейс для декоратора операций
 * Позволяет добавлять дополнительную функциональность к операциям
 */
#nullable disable
namespace Lab6_Resources.Decorators
{
    public interface IOperationDecorator
    {
        double Execute(double a, double b);
        string Name { get; }
    }
}
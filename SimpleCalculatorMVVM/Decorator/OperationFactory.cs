/*
 * OperationFactory.cs - Создаёт операции с декораторами
 */
#nullable disable
namespace Lab6_Resources.Decorators
{
    public static class OperationFactory
    {
        public static IOperationDecorator CreateOperation(string op, bool withLogging = true, bool withValidation = true)
        {
            IOperationDecorator operation = op switch
            {
                "+" => new BaseOperation("Сложение", (a, b) => a + b),
                "-" => new BaseOperation("Вычитание", (a, b) => a - b),
                "*" => new BaseOperation("Умножение", (a, b) => a * b),
                "/" => new BaseOperation("Деление", (a, b) =>
                {
                    if (b == 0) throw new DivideByZeroException();
                    return a / b;
                }),
                _ => new BaseOperation("Неизвестно", (a, b) => 0)
            };

            // Применяем декораторы (можно комбинировать)
            if (withValidation)
                operation = new ValidationOperationDecorator(operation);

            if (withLogging)
                operation = new LoggingOperationDecorator(operation);

            return operation;
        }
    }
}
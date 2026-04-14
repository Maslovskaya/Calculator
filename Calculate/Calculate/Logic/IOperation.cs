/*
 * IOperation.cs - Стратегии операций (Strategy Pattern)
 * 
 * Проблема, которую решает:
 * - Без Strategy: огромный switch/case с всеми операциями в одном месте
 * - С Strategy: каждая операция в отдельном классе, легко добавлять новые
 * 
 * Паттерн: Strategy (Стратегия)
 * - Определяет семейство алгоритмов (операций)
 * - Инкапсулирует каждый алгоритм в отдельный класс
 * - Делает их взаимозаменяемыми
 * 
 * Преимущества:
 * - Open/Closed Principle: новая операция = новый класс, старый код не меняется
 * - Single Responsibility: каждый класс отвечает за одну операцию
 * - Легко тестировать каждую операцию отдельно
 * 
 * Пример расширения:
 * - Чтобы добавить логарифм, создаём LogOperation : IOperation
 * - Не нужно трогать CalculatorLogic или другие операции
 */

namespace Calculator.Logic
{
    // === Стратегия (Interface) ===
    // Общий интерфейс для всех математических операций
    public interface IOperation
    {
        // Выполнить операцию над двумя числами
        double Execute(double a, double b);

        // Символ операции для отображения (например, "+", "×")
        string Symbol { get; }
    }

    // === Конкретная стратегия: Сложение ===
    public class AddOperation : IOperation
    {
        public string Symbol => "+";
        public double Execute(double a, double b) => a + b;
    }

    // === Конкретная стратегия: Вычитание ===
    public class SubtractOperation : IOperation
    {
        public string Symbol => "-";
        public double Execute(double a, double b) => a - b;
    }

    // === Конкретная стратегия: Умножение ===
    public class MultiplyOperation : IOperation
    {
        public string Symbol => "*";
        public double Execute(double a, double b) => a * b;
    }

    // === Конкретная стратегия: Деление ===
    public class DivideOperation : IOperation
    {
        public string Symbol => "/";
        public double Execute(double a, double b)
        {
            // Проверка на деление на ноль
            if (b == 0) throw new System.DivideByZeroException();
            return a / b;
        }
    }

    // === Пример для расширения (можно добавить для лабораторной) ===
    /*
    public class PowerOperation : IOperation
    {
        public string Symbol => "^";
        public double Execute(double a, double b) => Math.Pow(a, b);
    }
    */
}
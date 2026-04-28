/*
 * CalculatorButton.cs - Фабрика кнопок (Factory Method Pattern)
 * 
 * Проблема, которую решает:
 * - Без фабрики: создание кнопок размазано по коду, трудно изменить тип кнопки
 * - С фабрикой: все кнопки создаются в одном месте, легко добавлять новые типы
 * 
 * Паттерны:
 * 1. Factory Method - централизованное создание объектов
 * 2. Abstract Product - общий базовый класс для всех кнопок
 * 3. Strategy - каждая кнопка знает, как выполнить своё действие
 * 
 * Преимущества:
 * - Open/Closed Principle: можно добавить новый тип кнопки, не меняя существующий код
 * - Single Responsibility: каждая кнопка отвечает только за своё действие
 */

using Calculator.Logic;

namespace Calculator.Models
{
    // === Абстрактный продукт (Abstract Product) ===
    // Базовый класс для всех типов кнопок
    public abstract class CalculatorButton
    {
        // Текст, который отображается на кнопке
        public string Content { get; set; }

        // Ключ стиля для XAML (NumberButton, OperatorButton, FunctionButton)
        public string StyleKey { get; set; }

        // Метод выполнения действия (реализуется в наследниках)
        // Паттерн Strategy: каждая кнопка имеет свою стратегию выполнения
        public abstract void Execute(CalculatorLogic logic);
    }

    // === Конкретный продукт: Кнопка цифры ===
    public class NumberButton : CalculatorButton
    {
        // Значение цифры (0-9)
        public string Value { get; set; }

        // Передаёт цифру в логику для ввода
        public override void Execute(CalculatorLogic logic) => logic.InputNumber(Value);
    }

    // === Конкретный продукт: Кнопка операции ===
    public class OperatorButton : CalculatorButton
    {
        // Символ операции (+, -, *, /)
        public string Operator { get; set; }

        // Передаёт оператор в логику
        public override void Execute(CalculatorLogic logic) => logic.SetOperator(Operator);
    }

    // === Конкретный продукт: Функциональная кнопка ===
    public class FunctionButton : CalculatorButton
    {
        // Тип функции (Clear, Negate, Percent)
        public string FunctionType { get; set; }

        // Выполняет соответствующую функцию
        public override void Execute(CalculatorLogic logic)
        {
            switch (FunctionType)
            {
                case "Clear": logic.Clear(); break;      // Очистить всё
                case "Negate": logic.Negate(); break;    // Сменить знак
                case "Percent": logic.Percent(); break;  // Проценты
            }
        }
    }

    // === Фабрика (Factory Method) ===
    // Статический класс для создания кнопок
    public static class ButtonFactory
    {
        // Создать кнопку цифры
        public static CalculatorButton CreateNumber(string value) => new NumberButton
        { Content = value, Value = value, StyleKey = "NumberButton" };

        // Создать кнопку операции
        public static CalculatorButton CreateOperator(string op, string display) => new OperatorButton
        { Content = display, Operator = op, StyleKey = "OperatorButton" };

        // Создать функциональную кнопку
        public static CalculatorButton CreateFunction(string type, string display) => new FunctionButton
        { Content = display, FunctionType = type, StyleKey = "FunctionButton" };
    }
}
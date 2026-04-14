/*
 * CalculatorBuilder.cs - Построитель интерфейса (Builder Pattern)
 * 
 * Проблема, которую решает:
 * - Без Builder: создание кнопок размазано по коду, трудно изменить раскладку
 * - С Builder: конфигурация кнопок в одном месте, легко менять раскладку
 * 
 * Паттерн: Builder (Строитель)
 * - Позволяет создавать сложные объекты пошагово
 * - Отделяет конструирование объекта от его представления
 * 
 * Преимущества:
 * - Можно создать разные конфигурации калькулятора (обычный, инженерный)
 * - Код создания не смешивается с бизнес-логикой
 * - Легко добавить новые типы кнопок
 * 
 * Пример использования:
 * - Обычный калькулятор: builder.AddNumbers().AddOperators().AddFunctions()
 * - Инженерный: builder.AddNumbers().AddOperators().AddFunctions().AddScientific()
 */

using Calculator.Models;
using System.Collections.Generic;

namespace Calculator.Builders
{
    // === Продукт строителя ===
    // Хранит конфигурацию кнопок
    public class CalculatorLayout
    {
        public List<CalculatorButton> Buttons { get; set; } = new List<CalculatorButton>();
    }

    // === Builder (Строитель) ===
    public class CalculatorBuilder
    {
        private CalculatorLayout _layout;

        public CalculatorBuilder() => _layout = new CalculatorLayout();

        // === Пошаговое построение ===

        // Добавить цифровые кнопки (0-9)
        public CalculatorBuilder AddNumbers()
        {
            // Добавляем цифры по порядку (7-9, 4-6, 1-3, 0)
            for (int i = 7; i <= 9; i++)
                _layout.Buttons.Add(ButtonFactory.CreateNumber(i.ToString()));
            for (int i = 4; i <= 6; i++)
                _layout.Buttons.Add(ButtonFactory.CreateNumber(i.ToString()));
            for (int i = 1; i <= 3; i++)
                _layout.Buttons.Add(ButtonFactory.CreateNumber(i.ToString()));
            _layout.Buttons.Add(ButtonFactory.CreateNumber("0"));
            return this;  // Возвращаем this для цепочки вызовов (Fluent Interface)
        }

        // Добавить кнопки операций
        public CalculatorBuilder AddOperators()
        {
            _layout.Buttons.Add(ButtonFactory.CreateOperator("/", "÷"));
            _layout.Buttons.Add(ButtonFactory.CreateOperator("*", "×"));
            _layout.Buttons.Add(ButtonFactory.CreateOperator("-", "−"));
            _layout.Buttons.Add(ButtonFactory.CreateOperator("+", "+"));
            return this;
        }

        // Добавить функциональные кнопки
        public CalculatorBuilder AddFunctions()
        {
            _layout.Buttons.Add(ButtonFactory.CreateFunction("Clear", "C"));
            _layout.Buttons.Add(ButtonFactory.CreateFunction("Negate", "±"));
            _layout.Buttons.Add(ButtonFactory.CreateFunction("Percent", "%"));
            return this;
        }

        // === Пример расширения для инженерного калькулятора ===
        /*
        public CalculatorBuilder AddScientific()
        {
            _layout.Buttons.Add(ButtonFactory.CreateFunction("Sin", "sin"));
            _layout.Buttons.Add(ButtonFactory.CreateFunction("Cos", "cos"));
            _layout.Buttons.Add(ButtonFactory.CreateFunction("Sqrt", "√"));
            return this;
        }
        */

        // === Завершение построения ===
        public CalculatorLayout Build() => _layout;
    }
}
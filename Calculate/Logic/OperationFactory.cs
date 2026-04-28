/*
 * OperationFactory.cs - Фабрика операций (Factory Method Pattern)
 * 
 * Проблема, которую решает:
 * - Без фабрики: код создания операций размазан по программе
 * - С фабрикой: все операции создаются в одном месте
 * 
 * Паттерн: Factory Method (Фабричный метод)
 * - Определяет интерфейс для создания объекта
 * - Позволяет подклассам решать, какой класс instantiate
 * 
 * Преимущества:
 * - Централизованное создание объектов
 * - Легко изменить логику создания (например, добавить кэш)
 * - Код создания отделён от кода использования
 */

namespace Calculator.Logic
{
    // === Фабрика операций ===
    public static class OperationFactory
    {
        // Возвращает объект операции по символу
        // Паттерн Factory: скрывает детали создания объектов
        public static IOperation GetOperation(string op)
        {
            // Switch expression (C# 8.0+) - более читаемая версия switch
            return op switch
            {
                "+" => new AddOperation(),       // Создаём стратегию сложения
                "-" => new SubtractOperation(),  // Создаём стратегию вычитания
                "*" => new MultiplyOperation(),  // Создаём стратегию умножения
                "/" => new DivideOperation(),    // Создаём стратегию деления
                _ => null                        // Неизвестная операция
            };
        }

        // === Пример расширения ===
        /*
        public static IOperation GetOperation(string op)
        {
            return op switch
            {
                "+" => new AddOperation(),
                // ...
                "^" => new PowerOperation(),  // Добавляем новую операцию
                _ => null
            };
        }
        */
    }
}
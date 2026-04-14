/*
 * MainWindow.xaml.cs - Контроллер приложения
 * 
 * Этот класс связывает пользовательский интерфейс (XAML) с бизнес-логикой.
 * Он НЕ содержит логики вычислений - только передаёт команды от кнопок к логике.
 * 
 * Используемые паттерны:
 * - Observer (через события): логика уведомляет UI об изменениях
 * - Dependency Injection: логика внедряется в конструктор
 * 
 * Преимущества такого подхода:
 * 1. UI не зависит от деталей вычислений
 * 2. Легко заменить логику без изменения интерфейса
 * 3. Код тестируемый (можно тестировать логику отдельно)
 */

using Calculator.Logic;
using Calculator.Managers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        // Поле для хранения ссылки на логику калькулятора
        // private - инкапсуляция, внешний код не должен иметь доступ
        private readonly CalculatorLogic _logic;

        public MainWindow()
        {
            InitializeComponent();

            // Создаём экземпляр логики (Dependency Injection)
            _logic = new CalculatorLogic();

            // Подписываемся на события от логики (паттерн Observer)
            // Когда логика хочет обновить дисплей - она вызывает это событие
            _logic.DisplayUpdated += text => Display.Text = text;

            // Когда вычисление завершено - добавляем запись в историю
            _logic.CalculationPerformed += (expr, res) =>
            {
                // Перезагружаем источник данных для обновления UI
                HistoryList.ItemsSource = null;
                HistoryList.ItemsSource = HistoryManager.Instance.History;
            };

            // Инициализируем список истории (Singleton)
            HistoryList.ItemsSource = HistoryManager.Instance.History;
        }

        // === Обработчики нажатий кнопок ===
        // Все они просто делегируют вызов логике - никакой бизнес-логики здесь нет!

        // Ввод цифры (0-9 или запятая)
        private void Btn_Number_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            _logic.InputNumber(btn.Content.ToString());
        }

        // Ввод оператора (+, -, *, /)
        private void Btn_Operator_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            // Tag содержит символ операции для логики
            _logic.SetOperator(btn.Tag.ToString());
        }

        // Кнопка "Равно" - выполнить вычисление
        private void Btn_Equals_Click(object sender, RoutedEventArgs e) => _logic.Calculate();

        // Кнопка "C" - очистить всё
        private void Btn_Clear_Click(object sender, RoutedEventArgs e) => _logic.Clear();

        // Кнопка "±" - сменить знак
        private void Btn_Negate_Click(object sender, RoutedEventArgs e) => _logic.Negate();

        // Кнопка "%" - проценты
        private void Btn_Percent_Click(object sender, RoutedEventArgs e) => _logic.Percent();

        // Кнопка очистки истории
        private void Btn_ClearHistory_Click(object sender, RoutedEventArgs e) => HistoryManager.Instance.Clear();
    }
}
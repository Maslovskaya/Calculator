using lab3.Models;
using System;
using System.Collections.ObjectModel;

namespace lab3.Services
{
    /// <summary>
    /// Сервис для управления историей вычислений
    /// Отделяет логику работы с историей от ViewModel
    /// </summary>
    public class HistoryService
    {
        // Коллекция для хранения истории
        public ObservableCollection<HistoryItem> History { get; } = new ObservableCollection<HistoryItem>();

        /// <summary>
        /// Добавить запись в историю
        /// </summary>
        public void AddToHistory(string expression, double result)
        {
            History.Insert(0, new HistoryItem
            {
                Expression = expression,
                Result = result.ToString("G15"),
                Time = DateTime.Now
            });
        }

        /// <summary>
        /// Очистить всю историю
        /// </summary>
        public void Clear() => History.Clear();
    }
}
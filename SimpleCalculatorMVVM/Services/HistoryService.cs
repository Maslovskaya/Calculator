#nullable disable
using Lab6_Resources.Models;
using System;
using System.Collections.ObjectModel;

namespace Lab6_Resources.Services
{
    public class HistoryService
    {
        public ObservableCollection<HistoryItem> History { get; } = new ObservableCollection<HistoryItem>();

        public void AddToHistory(string expression, double result)
        {
            History.Insert(0, new HistoryItem
            {
                Expression = expression,
                Result = result.ToString("G15"),
                Time = DateTime.Now
            });
        }

        public void Clear() => History.Clear();
    }
}
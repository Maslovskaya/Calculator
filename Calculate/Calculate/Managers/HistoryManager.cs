/*
 * HistoryManager.cs - Менеджер истории (Singleton Pattern)
 * 
 * Проблема, которую решает:
 * - Без Singleton: история может быть в разных местах, данные рассинхронизируются
 * - С Singleton: гарантированно один экземпляр истории для всего приложения
 * 
 * Паттерн: Singleton (Одиночка)
 * - Гарантирует, что класс имеет только один экземпляр
 * - Предоставляет глобальную точку доступа к этому экземпляру
 * 
 * Преимущества:
 * - Экономия памяти (один объект на всё приложение)
 * - Контролируемый доступ к общим данным
 * - Потокобезопасность (lock для многопоточности)
 * 
 * Когда использовать:
 * - Логи, конфигурации, менеджеры ресурсов, кэш
 */

using System;
using System.Collections.ObjectModel;

namespace Calculator.Managers
{
    // === Элемент истории ===
    // Хранит информацию об одном вычислении
    public class HistoryItem
    {
        public string Expression { get; set; }  // Выражение (например, "5 + 3")
        public string Result { get; set; }       // Результат (например, "8")
        public DateTime Time { get; set; }       // Время вычисления
    }

    // === Singleton: Менеджер истории ===
    public sealed class HistoryManager
    {
        // Блокировка для потокобезопасности (на случай многопоточности)
        private static readonly object _lock = new object();

        // Единственный экземпляр класса (ленивая инициализация)
        private static HistoryManager _instance;

        // Коллекция истории (ObservableCollection для привязки к UI)
        public ObservableCollection<HistoryItem> History { get; } = new ObservableCollection<HistoryItem>();

        // Приватный конструктор - нельзя создать экземпляр извне
        private HistoryManager() { }

        // === Глобальная точка доступа (Singleton) ===
        public static HistoryManager Instance
        {
            get
            {
                lock (_lock)  // Защита от одновременного доступа
                {
                    // Создаём экземпляр только при первом обращении (Lazy Initialization)
                    if (_instance == null)
                        _instance = new HistoryManager();
                    return _instance;
                }
            }
        }

        // Добавить запись в историю
        public void AddEntry(string expression, string result)
        {
            // Вставляем в начало списка (новые записи сверху)
            History.Insert(0, new HistoryItem
            { Expression = expression, Result = result, Time = DateTime.Now });
        }

        // Очистить всю историю
        public void Clear() => History.Clear();
    }
}
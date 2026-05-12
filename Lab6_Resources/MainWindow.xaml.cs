#nullable disable

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Lab6_Resources.Dialogs;

namespace Lab6_Resources
{
    public partial class MainWindow : Window
    {
        private MediaPlayer _clickSound;
        private bool _isSoundEnabled = true;

        public MainWindow()
        {
            InitializeComponent();
            LoadSound();
            AddHandler(Button.PreviewMouseDownEvent, new MouseButtonEventHandler(OnButtonClick), true);
        }

        #region Звук

        private void LoadSound()
        {
            try
            {
                _clickSound = new MediaPlayer();
                _clickSound.Volume = 1.0;

                string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Sounds", "click.mp3");

                if (File.Exists(soundPath))
                {
                    _clickSound.Open(new Uri(soundPath, UriKind.Absolute));
                    MessageBox.Show("✅ Звук загружен!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"❌ Файл не найден:\n{soundPath}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnButtonClick(object sender, MouseButtonEventArgs e)
        {
            if (!_isSoundEnabled) return;

            if (e.OriginalSource is Button)
            {
                try
                {
                    if (_clickSound != null)
                    {
                        _clickSound.Position = TimeSpan.Zero;
                        _clickSound.Play();
                    }
                }
                catch { }
            }
        }

        #endregion

        #region Темы

        private void GlamourTheme_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Resources/Themes/GlamourTheme.xaml", "leopard.jpg");
        }

        private void GirlsTheme_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Resources/Themes/GirlsTheme.xaml", "hearts.png");
        }

        private void BoysTheme_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Resources/Themes/BoysTheme.xaml", "space.jpg");
        }

        private void DummyTheme_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Resources/Themes/GlamourTheme.xaml", null);

            if (DataContext is Lab6_Resources.ViewModels.CalculatorViewModel viewModel)
            {
                viewModel.SetDummyMode(true);
            }

            MessageBox.Show("🤓 РЕЖИМ ДЛЯ ЧАЙНИКОВ!\n\n2 + 6 = 26\n9 - 2 = 29\n2 * 3 = 222\n6 / 2 = 6?", "Для чайников", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        #endregion

        #region Применение темы

        private void ApplyTheme(string themePath, string imageName)
        {
            var oldTheme = Application.Current.Resources.MergedDictionaries.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Themes"));
            if (oldTheme != null)
                Application.Current.Resources.MergedDictionaries.Remove(oldTheme);

            try
            {
                var newTheme = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };
                Application.Current.Resources.MergedDictionaries.Add(newTheme);
                ChangeBackground(imageName);
                MessageBox.Show("✅ Тема применена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeBackground(string imageName)
        {
            var border = FindName("CalculatorBorder") as Border;
            if (border == null) return;

            if (string.IsNullOrEmpty(imageName))
            {
                border.Background = new SolidColorBrush(Color.FromRgb(44, 44, 44));
            }
            else
            {
                try
                {
                    var uri = new Uri($"pack://application:,,,/Lab6_Resources;component/Resources/Images/{imageName}", UriKind.Absolute);
                    var bitmap = new BitmapImage(uri);
                    border.Background = new ImageBrush(bitmap) { Stretch = Stretch.UniformToFill };
                }
                catch
                {
                    border.Background = new SolidColorBrush(Color.FromRgb(44, 44, 44));
                }
            }
        }

        #endregion

        #region Меню

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutDialog = new AboutDialog { Owner = this };
            aboutDialog.ShowDialog();
        }

        #endregion
    }
}
using System;
using System.Globalization;
using System.Windows.Forms;

namespace calculator1
{
    public partial class Form1 : Form
    {
        private readonly Calculator _calc = new Calculator();

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            textBox1.Text = _calc.Display;
        }

        private string _pendingExpression = ""; // хранит "3 +", "5 -", "-2 *"

        // --- Обработчики кнопок ---

        private void DigitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;
            string text = btn.Text;

            if (text == ".")
            {
                _calc.AppendDigit('.');
            }
            else if (char.IsDigit(text[0]))
            {
                _calc.AppendDigit(text[0]);
            }

            UpdateDisplay();
        }

        private void buttonSign_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_calc.CurrentInput))
                return;

            if (_calc.CurrentInput.StartsWith("-"))
                _calc.SetCurrentInput(_calc.CurrentInput.Substring(1));
            else
                _calc.SetCurrentInput("-" + _calc.CurrentInput);

            UpdateDisplay();
        }

        private void OperationButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string op = btn.Text;

            if (!string.IsNullOrEmpty(_calc.CurrentInput))
            {
                // Сохраняем: [текущее число] [операция]
                _pendingExpression = _calc.CurrentInput + " " + op;
            }

            _calc.SetOperation(op);
            UpdateDisplay();
        }

        private void EqualsButton_Click(object sender, EventArgs e)
        {
            string second = _calc.CurrentInput;
            string op = _calc.Operation; 

            // Собираем полное выражение:
            string fullExpr;
            if (!string.IsNullOrEmpty(_pendingExpression) && _pendingExpression.EndsWith(" " + op))
            {
                // Убираем операцию из _pendingExpression и добавляем второе число
                string firstPart = _pendingExpression.Substring(0, _pendingExpression.LastIndexOf(' '));
                fullExpr = $"{firstPart} {op} {second}";
            }
            else
            {
                // Если нет pending (например, сразу = после цифр), используем только текущее
                fullExpr = second;
            }

            var result = _calc.Calculate();

            if (result.HasValue)
            {
                textBox1.Text = result.Value.ToString(CultureInfo.InvariantCulture);
                LogCalculation(fullExpr, result.Value);
            }
            else
            {
                MessageBox.Show("Ошибка...", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _calc.Clear();
                UpdateDisplay();
                LogCalculation(fullExpr, null);
            }

            // Сбрасываем pending после =
            _pendingExpression = "";
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            _calc.Clear();
            UpdateDisplay();
        }

        private void SignButton_Click(object sender, EventArgs e)
        {
            _calc.ToggleSign();
            UpdateDisplay();
        }

        private void LogCalculation(string expression, double? result)
        {
            if (result.HasValue)
            {
                string line = $"{expression} = {result.Value:F10}".TrimEnd('0').TrimEnd('.');
                // Убираем лишние нули после точки: 4.5000000000 → 4.5, 3.0 → 3
                if (line.Contains("."))
                {
                    line = line.TrimEnd('0').TrimEnd('.');
                }
                textBox2.AppendText(line + Environment.NewLine);
            }
            else
            {
                textBox2.AppendText($"{expression} = ERROR" + Environment.NewLine);
            }

            // Автопрокрутка вниз
            textBox2.SelectionStart = textBox2.Text.Length;
            textBox2.ScrollToCaret();
        }
        // --- Клавиатурный ввод ---

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Цифры (основная клавиатура и NumPad)
            if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) ||
                (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
            {
                char digit = e.KeyCode >= Keys.NumPad0
                    ? (char)(e.KeyCode - Keys.NumPad0 + '0')
                    : (char)(e.KeyCode - Keys.D0 + '0');
                _calc.AppendDigit(digit);
                UpdateDisplay();
                e.SuppressKeyPress = true;
            }
            // Точка
            else if (e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Oemcomma) // добавлен случай для запятой
            {
                _calc.AppendDigit('.');
                UpdateDisplay();
                e.SuppressKeyPress = true;
            }
            // Операции
            else if (e.KeyCode == Keys.Add) { _calc.SetOperation("+"); UpdateDisplay(); e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.Subtract) { _calc.SetOperation("-"); UpdateDisplay(); e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.Multiply) { _calc.SetOperation("*"); UpdateDisplay(); e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.Divide) { _calc.SetOperation("/"); UpdateDisplay(); e.SuppressKeyPress = true; }
            // Равно и Сброс
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space) { EqualsButton_Click(null, null); e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.Escape) { ClearButton_Click(null, null); e.SuppressKeyPress = true; }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }
    }
}
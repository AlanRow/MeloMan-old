using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SpectrumVisor
{
    //окно создания нового сигнала
    class AddSignalDialog : Form
    {
        private SignalOptions opts;
        private Label errorLabel;

        public AddSignalDialog(SignalController signal)
        {
            //фиксирование размеров
            Width = 400;
            Height = 600;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            opts = new SignalOptions(0, signal.Internal.Size);

            var table = new TableLayoutPanel();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            var startField = new SubscribedField("Начало: ", "Начало сигнала должно быть целым положительным числом.", opts.Start.ToString());
            var durField = new SubscribedField("Продолжительность:", "Продолжительность сигнала должна быть целым положительным числом.", opts.Duration.ToString());
            var freqField = new SubscribedField("Частота повторения ():", "Частота повторения  сигнала должна быть действительным числом.", opts.Freq.ToString());
            var multField = new SubscribedField("Множитель:", "Множитель сигнала должен быть действительным числом.", opts.Mult.ToString());
            var constField = new SubscribedField("Константа:", "Константа сигнала должна быть действительным числом. ", opts.Const.ToString());

            var container = new OptionsParser
            {
                Start = startField,
                Duration = durField,
                Frequency = freqField,
                Mult = multField,
                Const = constField
            };

            var okButton = new Button
            {
                Text = "Создать"
            };
            okButton.Click += (sender, ev) =>
            {
                opts = container.GetOptions();

                if (opts == null)
                {
                    ThrowErrors(container.ErrorMessages);
                }
                else
                {
                    try
                    {
                        signal.AddSignal(new SinSignal(opts));
                        Close();
                    } catch (ArgumentException ex)
                    {
                        ThrowErrors(ex.Message);
                    }
                }
            };


            var cancelButton = new Button
            {
                Text = "Отмена"
            };
            cancelButton.Click += (sender, ev) =>
            {
                Close();
            };

            //надпись об ошибке
            errorLabel = new Label
            {
                Text = "",
                ForeColor = Color.Red,
                AutoSize = true
            };
            
            table.Controls.Add(startField, 0, 0);
            table.Controls.Add(durField, 1, 0);
            table.Controls.Add(freqField, 0, 1);
            table.Controls.Add(multField, 1, 1);
            table.Controls.Add(constField, 0, 2);

            table.Controls.Add(okButton, 0, 3);
            table.Controls.Add(cancelButton, 1, 3);

            table.Controls.Add(errorLabel, 0, 4);

            table.Dock = DockStyle.Fill;
            Controls.Add(table);
        }

        private void ThrowErrors(List<string> errorMessage)
        {
            var errors = new StringBuilder();
            errors.Append(errorMessage[0]);

            for (var i = 1; i < errorMessage.Count; i++)
            {
                errors.Append("\n\r");
                errors.Append(errorMessage);
            }

            errorLabel.Text = errors.ToString();
            Invalidate();
        }

        private void ThrowErrors(string errorMessage)
        {
            var list = new List<string>();
            list.Add(errorMessage);
            ThrowErrors(list);
        }
    }

    class SubscribedField : Panel
    {
        public TextBox Field { get; private set;}
        public string ErrorMessage { get; private set; }

        public SubscribedField(string label, string message, string defaultValue)
        {
            ErrorMessage = message;

            var name = new Label
            {
                Font = new System.Drawing.Font("Arial", 10),
                Text = label,
                AutoSize = true
            };
            
            Field = new TextBox
            {
                Width = 200,
                Text = defaultValue,
                Location = new Point(0, name.Height)
            };

            Controls.Add(name);
            Controls.Add(Field);
        }
    }
}

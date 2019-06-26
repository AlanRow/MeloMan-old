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

        //private string name;
        //private double start;
        //private double dur;
        //private double mult;
        //private double freq;
        //private double c;

        public AddSignalDialog(SignalManager signals)
        {
            //фиксирование размеров
            Width = 400;
            Height = 600;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            opts = new SignalOptions(0, signals.Size);

            var table = new TableLayoutPanel();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            table.Controls.Add(IFG.InitInputField("Начало:", (val) => {
                try
                {
                    opts.Start = Int32.Parse(val.ToString());
                } catch (FormatException ex)
                {
                    ThrowError("Начало сигнала должно быть целым числом.");
                } catch (ArgumentException ex)
                {
                    ThrowError("Начало сигнала должно быть целым положительным числом.");
                }
            }, opts.Start), 0, 0);

            table.Controls.Add(IFG.InitInputField("Продолжительность:", (val) => {
                try
                {
                    opts.Duration = Int32.Parse(val.ToString());
                }
                catch (FormatException ex)
                {
                    ThrowError("Продолжительность сигнала должна быть целым числом.");
                }
                catch (ArgumentException ex)
                {
                    ThrowError("Продолжительность сигнала должна быть целым положительным числом.");
                }
            }, opts.Duration), 0, 1);
            
            table.Controls.Add(new Panel(), 0, 2);

            table.Controls.Add(IFG.InitInputField("Частота повторения ():", (val) => {
                try
                {
                    opts.Freq = Double.Parse(val.ToString());
                }
                catch (FormatException ex)
                {
                    ThrowError("Частота повторения  сигнала должна быть действительным числом.");
                }
                catch (ArgumentException ex)
                {
                    ThrowError("Частота повторения  сигнала должна быть действительным числом.");
                }
            }, opts.Freq), 1, 0);

            table.Controls.Add(IFG.InitInputField("Множитель:", (val) => {
                try
                {
                    opts.Mult = Double.Parse(val.ToString());
                }
                catch (FormatException ex)
                {
                    ThrowError("Множитель сигнала должен быть действительным числом.");
                }
                catch (ArgumentException ex)
                {
                    ThrowError("Множитель сигнала должен быть действительным числом.");
                }
            }, opts.Mult), 1, 1);

            table.Controls.Add(IFG.InitInputField("Константа:", (val) => {
                try
                {
                    opts.Const = Double.Parse(val.ToString());
                }
                catch (FormatException ex)
                {
                    ThrowError("Константа сигнала должна быть действительным числом.");
                }
                catch (ArgumentException ex)
                {
                    ThrowError("Константа сигнала должна быть действительным числом.");
                }
            }, opts.Const), 1, 2);

            var okButton = new Button
            {
                Text = "Создать"
            };
            okButton.Click += (sender, ev) =>
            {
                signals.AddSignal(new SinSignal(opts));
                Close();
            };

            table.Controls.Add(okButton, 0, 3);

            var cancelButton = new Button
            {
                Text = "Отмена"
            };
            cancelButton.Click += (sender, ev) =>
            {
                Close();
            };
            table.Controls.Add(cancelButton, 1, 3);

            errorLabel = new Label
            {
                Text = "",
                ForeColor = Color.Red
            };

            table.Controls.Add(errorLabel, 0, 4);

            table.Dock = DockStyle.Fill;

            Controls.Add(table);
        }

        private void ThrowError(string errorMessage)
        {
            errorLabel.Text = errorMessage;
            Invalidate();
        }
    }

    class SubscribedField : Panel
    {
        public TextBox Field { get; private set;}

        public SubscribedField(string label)
        {
            Controls.Add(new Label
            {
                Font = new System.Drawing.Font("Arial", 16),
                Text = label
            });

            Field = new TextBox();
            Controls.Add(Field);
        }
    }
}

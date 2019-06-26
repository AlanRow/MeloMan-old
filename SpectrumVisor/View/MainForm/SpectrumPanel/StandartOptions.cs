using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    class StandartOptionsPanel : OptionsPanel
    {
        private TransformManager transform;
        private StdOptions opts;

        public StandartOptionsPanel(TransformManager manager) : base()
        {
            transform = manager;

            var table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            //число частот
            var freqCountPan = IFG.InitInputField("Кол-во частот:", (box) =>
            {
                var iValue = 0;
                if (!TryAnyParse(box.Text, ref iValue))
                {
                    MessageBox.Show("Количество частот должно быть целым числом.");
                    box.Text = opts.CountFreq.ToString();
                }
                else
                {
                    try
                    {
                        opts.CountFreq = iValue;
                    } catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                        box.Text = opts.CountFreq.ToString();
                    }
                }
            }, opts.CountFreq); 


            //шаг частоты
            var freqStartPan = IFG.InitInputField("Начальная частота:", (box) =>
            {
                var dValue = 0d;
                if (!TryAnyParse(box.Text, ref dValue))
                {
                    MessageBox.Show("Стартовая частота должна быть десятичным числом.");
                    box.Text = opts.StartFreq.ToString();
                }
                else
                {
                    try
                    {
                        opts.StartFreq = dValue;
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                        box.Text = opts.StartFreq.ToString();
                    }
                }
            }, opts.StartFreq);

            //стартовая частота
            var freqStepPan = IFG.InitInputField("Сдвиг частоты:", (box) =>
            {
                var dValue = 0d;
                if (!TryAnyParse(box.Text, ref dValue))
                {
                    MessageBox.Show("Сдвиг частоты должен быть десятичным числом.");
                    box.Text = opts.StepFreq.ToString();
                }
                else
                {
                    try
                    {
                        opts.StepFreq = dValue;
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                        box.Text = opts.StepFreq.ToString();
                    }
                }
            }, opts.StartFreq);

            //кнопка обновления
            var updateButton = new Button
            {
                Text = "Обновить",
            };

            updateButton.Click += (sender, ev) =>
            {
                Invalidate();
            };

            table.Controls.Add(freqStartPan, 0, 0);
            table.Controls.Add(freqStepPan, 1, 0);
            table.Controls.Add(freqCountPan, 0, 1);
            table.Controls.Add(new Panel(), 1, 1);
            table.Controls.Add(updateButton, 0, 2);
            table.Controls.Add(new Panel(), 1, 2);
            table.SetColumnSpan(updateButton, 2);
            Controls.Add(table);
        }

        static private bool TryAnyParse(string str, ref double val)
        {
            try
            {
                val = double.Parse(str, new CultureInfo("en-us"));
            }
            catch (FormatException ex1)
            {
                try
                {
                    val = double.Parse(str, new CultureInfo("ru-ru"));
                } catch (FormatException ex2)
                {
                    return false;
                }
            }
            return true;
        }

        static private bool TryAnyParse(string str, ref int val)
        {
            try
            {
                val = int.Parse(str, new CultureInfo("en-us"));
            }
            catch (FormatException ex1)
            {
                try
                {
                    val = int.Parse(str, new CultureInfo("ru-ru"));
                }
                catch (FormatException ex2)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

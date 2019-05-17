using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    class OptionsPanel : Panel
    {
        private TransformManager transform;

        private Tuple<int, bool> freqCount;
        private Tuple<double, bool> freqStart;
        private Tuple<double, bool> freqStep;

        public OptionsPanel(TransformManager manager) : base()
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
            var freqCountPan = IFG.InitInputField("Кол-во частот:", (value) =>
            {
                var iValue = 0;
                if (int.TryParse(value, out iValue))
                    freqCount = Tuple.Create(iValue, true);
                else
                    freqCount = Tuple.Create(freqCount.Item1, false);
            });


            //шаг частоты
            var freqStartPan = IFG.InitInputField("Начальная частота:", (value) =>
            {
                var dValue = 0d;
                if (double.TryParse(value, out dValue))
                    freqStart = Tuple.Create(dValue, true);
                else
                    freqStart = Tuple.Create(freqStart.Item1, false);
            });

            //стартовая частота
            var freqStepPan = IFG.InitInputField("Изменение частоты:", (value) =>
            {
                var dValue = 0d;
                if (double.TryParse(value, out dValue))
                    freqStep = Tuple.Create(dValue, true);
                else
                    freqStep = Tuple.Create(freqStep.Item1, false);
            });

            //кнопка обновления
            var updateButton = new Button
            {
                Text = "Обновить",
            };

            updateButton.Click += (sender, ev) =>
            {
                if (freqStart.Item2 && freqStep.Item2 && freqCount.Item2)
                    transform.SetParams(freqCount.Item1, freqStep.Item1, freqStart.Item1);
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
    }
}

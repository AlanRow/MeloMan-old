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

        private int freqCount;
        private double freqStart;
        private double freqStep;

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

            freqCount = transform.FreqSize;
            freqStart = transform.StartFreq;
            freqStep = transform.FreqStep;

            //число частот
            var freqCountPan = IFG.InitInputField("Кол-во частот:", (value) =>
            {
                var iValue = 0;
                if (int.TryParse(value, out iValue))
                    freqCount = iValue;
            },
            freqCount);


            //шаг частоты
            var freqStartPan = IFG.InitInputField("Начальная частота:", (value) =>
            {
                var dValue = 0d;
                if (double.TryParse(value, out dValue))
                    freqStart = dValue;
            },
            freqStart);

            //стартовая частота
            var freqStepPan = IFG.InitInputField("Изменение частоты:", (value) =>
            {
                var dValue = 0d;
                if (double.TryParse(value, out dValue))
                    freqStep = dValue;
            },
            freqStep);

            //кнопка обновления
            var updateButton = new Button
            {
                Text = "Обновить",
            };

            updateButton.Click += (sender, ev) =>
            {
                //TODO: здесь должен быть ворох исключений
                transform.SetParams(freqCount, freqStep, freqStart);
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

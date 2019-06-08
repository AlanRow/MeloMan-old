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
        private string name;
        private double start;
        private double dur;
        private double mult;
        private double freq;
        private double c;

        public AddSignalDialog(SignalManager signals)
        {
            //фиксирование размеров
            Width = 400;
            Height = 600;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            name = "no_name";
            start = 0;
            dur = signals.Size;
            mult = 1;
            freq = 8 / dur;
            c = 0;

            var table = new TableLayoutPanel();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            table.Controls.Add(InitStartField(), 0, 0);
            table.Controls.Add(InitDurField(), 0, 1);
            table.Controls.Add(new Panel(), 0, 2);
            table.Controls.Add(InitFreqField(), 1, 0);
            table.Controls.Add(InitMultField(), 1, 1);
            table.Controls.Add(InitConstField(), 1, 2);

            var okButton = new Button
            {
                Text = "Создать"
            };
            okButton.Click += (sender, ev) =>
            {
                signals.AddSignalBySize((int)start, (int)dur, 0, freq, mult, c, 0);
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

            table.Dock = DockStyle.Fill;

            Controls.Add(table);
        }

        private Panel InitStartField()
        {
            return IFG.InitDoubleField("Начало: ", (dStart) => { start = dStart; }, start);
        }

        private Panel InitDurField()
        {
            return IFG.InitDoubleField("Длительность: ", (dDur) => { dur = dDur; }, dur);
        }

        private Panel InitMultField()
        {
            return IFG.InitDoubleField("Множитель: ", (dMult) => { mult = dMult; }, mult);
        }

        private Panel InitConstField()
        {
            return IFG.InitDoubleField("Константа: ", (dConst) => { c = dConst; }, c);
        }

        private Panel InitFreqField()
        {
            return IFG.InitDoubleField("Частота (тактов в повторе): ", (dTimes) => { freq = 1/dTimes; }, freq);
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

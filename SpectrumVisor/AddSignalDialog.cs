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
        public AddSignalDialog(SignalManager signals)
        {
            Width = 400;
            Height = 600;

            /*var table = new TableLayoutPanel();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            var title = new Label
            {
                Font = new System.Drawing.Font("Arial", 22),
                Text = "Введите параметры сигнала"
            };
            table.Controls.Add(title, 0, 0);
            table.SetColumnSpan(title, 2);

            var startField*/

            var OKButton = new Button
            {
                Location = new Point(100, 500),
                Text = "Создать",
            };
            OKButton.Click += (sender, ev) =>
            {
                signals.AddSignalBySize(0, 128, 0, 1, 1, 0, 0);
                Close();
            };

            var CancelButton = new Button
            {
                Location = new Point(250, 500),
                Text = "Отменить"
            };
            CancelButton.Click += (sender, ev) =>
            {
                Close();
            };


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

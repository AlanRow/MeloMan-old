using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    class SignalDeleteConfirm : Form
    {
        public SignalDeleteConfirm(SignalManager manager, SinSignal signal)
        {
            MaximumSize = new Size(400, 300);
            MinimumSize = new Size(400, 300);

            var message = new Label
            {
                Width = 250,
                Text = "Вы уверены что хотите удалить сигнал " + signal.Options.GetTextFormula() + " из списка сигналов.",
                Location = new Point(50, 100)
            };

            var okButton = new Button
            {
                Location = new Point(100, 150),
                Text = "Да"
            };
            okButton.Click += (sender, ev) => {
                manager.DeleteSignal(signal);
                Close();
            };

            var cancelButton = new Button()
            {
                Location = new Point(200, 150),
                Text = "Нет"
            };
            cancelButton.Click += (sender, ev) => { Close(); };

            Controls.Add(message);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    class SignalsList : FlowLayoutPanel
    {
        public SignalsList(SignalManager manager) : base()
        {
            FlowDirection = FlowDirection.TopDown;
            //Height = 300;

            for (var i = 0; i < manager.Signals.Count; i++)
            {
                var j = i;
                var signalFrame = new Panel
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Height = 60,
                    Width = Width
                };

                var delButton = new Button
                {
                    Dock = DockStyle.Bottom,
                    Text = "Удалить"
                };

                delButton.Click += (sender, ev) =>
                {
                    new SignalDeleteConfirm(manager, j).ShowDialog();
                };

                var formula = new Label
                {
                    Dock = DockStyle.Top,
                    Text = manager.Signals[j].GetTextFormula(),
                    Font = new Font("Arial", 10)
                };

                signalFrame.Controls.Add(formula);
                signalFrame.Controls.Add(delButton);
                Controls.Add(signalFrame);
            }

            var addButton = new Button
            {
                Height = 40,
                Width = Width,
                Text = "Add signal"
            };

            addButton.Click += (sender, ev) =>
            {
                var creatingDialog = new AddSignalDialog(manager).ShowDialog();
            };

            Controls.Add(addButton);
            //this.VScroll = true;
            //VerticalScroll.Enabled = true;
            //AutoScroll = true;
        }
    }
}

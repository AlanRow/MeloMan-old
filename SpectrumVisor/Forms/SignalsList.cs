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
        private static int frameHeight = 100;
        private Button addButton;

        public SignalsList(SignalManager manager) : base()
        {
            Height = (manager.Signals.Count + 1) * frameHeight + 50;
            FlowDirection = FlowDirection.TopDown;

            ScrollBar scroll = new VScrollBar();
            scroll.Dock = DockStyle.Right;
            scroll.Scroll += (sender, ev) =>
            {
                VerticalScroll.Value = scroll.Value;
            };

            manager.AddedSignal += (values, index) =>
            {
                Controls.Remove(addButton);
                Controls.Add(GetSignalFrame(manager, index));
                Controls.Add(addButton);
            };

            manager.DeletedSignal += (values, index) =>
            {
                Controls.RemoveAt(index);
            };

            addButton = GetAddButton(manager);

            for (var i = 0; i < manager.Signals.Count; i++)
            {
                var j = i;
                var signalFrame = GetSignalFrame(manager, j);
             }
            
            Controls.Add(addButton);
            Controls.Add(scroll);
        }

        private Button GetAddButton(SignalManager manager)
        {
            var button = new Button
            {
                Height = frameHeight - 10,
                Width = Width - 20,
                Text = "Добавить"
            };

            button.Click += (sender, ev) =>
            {
                var creatingDialog = new AddSignalDialog(manager).ShowDialog();
            };

            return button;
        }

            private Panel GetSignalFrame(SignalManager manager, int i)
        {
            var frame = new Panel
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Height = frameHeight,
                    Width = Width - 20
                };

            var delButton = new Button
                {
                    Dock = DockStyle.Bottom,
                    Text = "Удалить"
                };

            delButton.Click += (sender, ev) =>
                {
                    new SignalDeleteConfirm(manager, i).ShowDialog();
                };

            var formula = new Label
                {
                    Dock = DockStyle.Top,
                    Text = manager.Signals[i].GetTextFormula(),
                    Font = new Font("Arial", 10)
                };

            frame.Controls.Add(formula);
            frame.Controls.Add(delButton);
            Controls.Add(frame);

            return frame;
        }
    }
}

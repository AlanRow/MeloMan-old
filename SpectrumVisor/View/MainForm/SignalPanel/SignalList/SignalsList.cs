using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    //выводит список сигналов с возможностью их удаления
    class SignalsList : FlowLayoutPanel
    {
        private static int frameHeight = 100;
        private Button addButton;

        public SignalsList(ApplicationState state) : base()
        {
            var manager = state.InternalState.Signal;

            Height = (manager.Signals.Count + 1) * frameHeight + 50;
            FlowDirection = FlowDirection.LeftToRight;

            //ScrollBar scroll = new VScrollBar();
            //scroll.Dock = DockStyle.Right;
            //scroll.Scroll += (sender, ev) =>
            //{
            //    VerticalScroll.Value = scroll.Value;
            //};

            addButton = GetAddButton(manager);

            for (var i = 0; i < manager.Signals.Count; i++)
            {
                var j = i;
                var signalFrame = GetSignalFrame(manager, manager.Signals[i], j);
             }
            
            Controls.Add(addButton);
            //Controls.Add(scroll);
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

        private Panel GetSignalFrame(SignalManager manager, SinSignal signal, int i)
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
                    new SignalDeleteConfirm(manager, signal).ShowDialog();
                };

            var formula = new Label
                {
                    Dock = DockStyle.Top,
                    Text = manager.Signals[i].Options.GetTextFormula(),
                    Font = new Font("Arial", 10)
                };

            frame.Controls.Add(formula);
            frame.Controls.Add(delButton);
            Controls.Add(frame);

            return frame;
        }
    }
}

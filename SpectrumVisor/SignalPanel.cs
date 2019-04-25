using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace SpectrumVisor
{
    enum SignalViewType
    {
        Signals,
        Sum
    }

    class SignalPanel : Panel
    {
        private SignalManager signalState;
        private SignalViewType viewType;
        private Dictionary<SignalViewType, Chart> signalViewes;

        private TableLayoutPanel signalsList;
        private Chart currentView;

        public SignalPanel(SignalManager signals)
        {
            signalState = signals;
            viewType = SignalViewType.Signals;
            signalViewes = new Dictionary<SignalViewType, Chart>();
            signalsList = new TableLayoutPanel();
            reconstruct();
        }

        public void DeleteSignal(int i)
        {
            MessageBox.Show("Вы уверены, что хотите удалить сигнал с номером " + i + "?", "Подтвердите удаление",
                MessageBoxButtons.OKCancel);
            signalState.DeleteSignal(i);
            reconstruct();
        }

        public void AddSignal()
        {
            var creatingDialog = new AddSignalDialog(signalState);
            creatingDialog.ShowDialog();
            reconstruct();
        }

        private void reconstruct()
        {
            signalViewes[SignalViewType.Signals] = new SignalChart(signalState.Signals, signalState.Size);
            signalViewes[SignalViewType.Sum] = new SignalChart(signalState.Sum);

            signalsList = new TableLayoutPanel();
            signalsList.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (var i = 0; i < signalState.Signals.Count; i++)
            {
                var signalFrame= new Panel
                {
                    BorderStyle = BorderStyle.FixedSingle,
                };

                var delButton = new Button
                {
                    Dock = DockStyle.Bottom,
                    Text = "Delete"
                };
                delButton.Click += (sender, ev) =>
                {
                    var j = i;
                    DeleteSignal(j);
                };

                signalFrame.Controls.Add(delButton);
                signalsList.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
                signalsList.Controls.Add(signalFrame, 0, i);
            }
            
            signalsList.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            var addButton = new Button
            {
                Dock = DockStyle.Fill,
                Text = "Add signal"
            };
            addButton.Click += (sender, ev) => 
            {
                AddSignal();
            };
            signalsList.Controls.Add(addButton, 0, signalState.Signals.Count);
            signalsList.AutoScroll = true;

            redraw();
        }

        private void redraw()
        {
            Controls.Clear();
            currentView = signalViewes[viewType];

            currentView.Width = Width;
            currentView.Height = Width;

            Controls.Add(currentView);

            var viewSwitchButton = new Button();
            switch (viewType)
            {
                case SignalViewType.Signals:
                    viewSwitchButton.Text = "Суммарный";
                    viewSwitchButton.Click += (sender, ev) =>
                    {
                        viewType = SignalViewType.Sum;
                        redraw();
                    };
                    break;
                case SignalViewType.Sum:
                    viewSwitchButton.Text = "Разделенный";
                    viewSwitchButton.Click += (sender, ev) =>
                    {
                        viewType = SignalViewType.Signals;
                        redraw();
                    };
                    break;
            }
            Controls.Add(viewSwitchButton);

            signalsList.Location = new Point(0, Width + 10);
            signalsList.Width = Width;
            signalsList.Height = Height - Width - 20;
            Controls.Add(signalsList);
        }
    }
}

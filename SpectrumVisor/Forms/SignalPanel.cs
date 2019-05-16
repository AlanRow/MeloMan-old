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
        
        private FlowLayoutPanel signalsList;
        private Chart currentView;

        public SignalPanel(SignalManager signals)
        {
            signalState = signals;
            viewType = SignalViewType.Signals;

            var mainPan = new FlowLayoutPanel();
            mainPan.FlowDirection = FlowDirection.TopDown;

            signalViewes = new Dictionary<SignalViewType, Chart>();
            signalsList = new FlowLayoutPanel();

            reconstruct();
        }

        public void DeleteSignal(int i)
        {
            MessageBox.Show("Вы уверены, что хотите удалить сигнал с номером " + i + "?", "Подтвердите удаление",
                MessageBoxButtons.OKCancel);
            signalState.DeleteSignal(i);
            reconstruct();
        }

        public void ShowAddSignalDialog()
        {
            var creatingDialog = new AddSignalDialog(signalState, this);
            creatingDialog.ShowDialog();
            reconstruct();
        }

        public void AddSignal(int start, int dur, double offset, double freq, double mult, double constant, double fading)
        {
            signalState.AddSignalBySize(start, dur, offset, freq, mult, constant, fading);
            reconstruct();
        }

        private void reconstruct()
        {
            signalViewes[SignalViewType.Signals] = new SignalChart(signalState.Signals, signalState.Size);
            signalViewes[SignalViewType.Sum] = new SignalChart(signalState.Sum);

            signalsList = new FlowLayoutPanel();

            for (var i = 0; i < signalState.Signals.Count; i++)
            {
                var j = i;
                var signalFrame= new Panel
                {
                    BorderStyle = BorderStyle.FixedSingle,
                };

                var delButton = new Button
                {
                    Dock = DockStyle.Bottom,
                    Text = "Удалить"
                };
                delButton.Click += (sender, ev) =>
                {
                    DeleteSignal(j);
                };

                var formula = new Label
                {
                    Dock = DockStyle.Top,
                    Text = signalState.Signals[j].GetTextFormula(),
                    Font = new Font("Arial", 10)
                };

                signalFrame.Controls.Add(formula);
                signalFrame.Controls.Add(delButton);
                signalsList.Controls.Add(signalFrame);
            }
            
            var addButton = new Button
            {
                Dock = DockStyle.Fill,
                Text = "Add signal"
            };
            addButton.Click += (sender, ev) => 
            {
                ShowAddSignalDialog();
            };
            signalsList.Controls.Add(addButton);
            signalsList.AutoScroll = true;

            redraw();
        }

        private void redraw()
        {
            var mainPan = new FlowLayoutPanel();

            Controls.Clear();
            currentView = signalViewes[viewType];

            //currentView.Width = 200;
            //currentView.Height = 200;
            mainPan.Controls.Add(currentView);

            //увеличенный график
            var dialog = new Form();
            //dialog.Width = 800;
            //dialog.Height = 800;

            //отображается при клике по уменьшенному
            currentView.MouseClick += (sender, ev) =>
            {
                var bigChart = new SignalChart(signalState.Signals, signalState.Size);
                bigChart.Width = 750;
                bigChart.Height = 750;
                dialog.Controls.Add(bigChart);
                dialog.ShowDialog();
            };

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
            viewSwitchButton.Location = new Point(0, 200);
            Controls.Add(viewSwitchButton);

            signalsList.Location = new Point(0, 250);
            signalsList.Width = Width;
            signalsList.Height = Height - Width - 20;
            Controls.Add(signalsList);
        }
    }
}

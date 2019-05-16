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

        private SignalsList signalsList;
        private Chart currentView;
        private Button viewSwitchButton;

        public SignalPanel(SignalManager signals) : base()
        {
            var mainTable = new TableLayoutPanel();

            signalState = signals;
            viewType = SignalViewType.Signals;

            signalViewes = new Dictionary<SignalViewType, Chart>();
            signalsList = new SignalsList(signals);

            viewSwitchButton = new Button();
            viewSwitchButton.Location = new Point(0, 0);
            switch (viewType)
            {
                case SignalViewType.Signals:
                    viewSwitchButton.Text = "Суммарный";
                    viewSwitchButton.Click += (sender, ev) =>
                    {
                        viewType = SignalViewType.Sum;
                        Invalidate();
                    };
                    break;
                case SignalViewType.Sum:
                    viewSwitchButton.Text = "Разделенный";
                    viewSwitchButton.Click += (sender, ev) =>
                    {
                        viewType = SignalViewType.Signals;
                        Invalidate();
                    };
                    break;
            }


            viewSwitchButton.Click += (sender, ev) =>
            {
                //увеличенный график
                var dialog = new Form();

                //отображается при клике по уменьшенному
                currentView.MouseClick += (obj, even) =>
                {
                    var bigChart = new SignalChart(signalState.Signals, signalState.Size);
                    bigChart.Width = 750;
                    bigChart.Height = 750;
                    dialog.Controls.Add(bigChart);
                    dialog.ShowDialog();
                };
            };


            Reconstruct();

            signals.AddedSignal += (values, index) =>
            {
                Reconstruct();
                Invalidate();
            };

            signals.DeletedSignal += (values, index) =>
            {
                Reconstruct();
                Invalidate();
            };

            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 55));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            viewSwitchButton.Dock = DockStyle.Fill;
            mainTable.Controls.Add(viewSwitchButton, 0, 0);

            foreach (var view in signalViewes.Values)
            {
                view.Visible = false;
                view.Dock = DockStyle.Fill;
                mainTable.Controls.Add(view, 0, 1);
            }

            currentView = signalViewes[viewType];
            signalsList.Dock = DockStyle.Fill;
            mainTable.Controls.Add(signalsList, 0, 2);
            Controls.Add(mainTable);
        }

        private void Reconstruct()
        {
            signalViewes[SignalViewType.Signals] = new SignalChart(signalState.Signals, signalState.Size);
            signalViewes[SignalViewType.Sum] = new SignalChart(signalState.Sum);
            //currentView.SetBounds(0, 25, Width, (int)(Height * 40 / 100));
            //signalsList.SetBounds(0, Height * 40 / 100 + 50, Width, Height * 55 / 100);
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            currentView.Visible = false;
            signalViewes[viewType].Visible = true;
            currentView = signalViewes[viewType];
        }
    }
}

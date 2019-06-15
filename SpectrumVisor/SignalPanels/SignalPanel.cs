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
            signalState = signals;
            viewType = SignalViewType.Signals;

            signalViewes = new Dictionary<SignalViewType, Chart>();
            InitCharts();
            currentView = signalViewes[viewType];

            signalsList = new SignalsList(signals);
            viewSwitchButton = new Button();
            viewSwitchButton = GetSwitchButton();

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

            Controls.Add(currentView);
            Controls.Add(viewSwitchButton);
            Controls.Add(signalsList);

            SizeChanged += (sender, ev) =>
            {
                currentView.SetBounds(0, 0, Height / 2, Height / 2);

                viewSwitchButton.Location = new Point(currentView.Width - viewSwitchButton.Width, 
                                                      currentView.Height);

                var listPrefs = signalsList.PreferredSize;
                signalsList.SetBounds(0, currentView.Height + viewSwitchButton.Height,
                                      listPrefs.Width, listPrefs.Height);
            };
        }

        private void InitCharts()
        {
            Reconstruct();
            
            foreach (var view in signalViewes.Values)
            {
                //увеличенный график
                var dialog = new Form();

                //отображается при клике по уменьшенному
                view.MouseClick += (obj, even) =>
                {
                    var bigChart = new SignalChart(signalState.Signals, signalState.Size);
                    bigChart.Width = 750;
                    bigChart.Height = 750;
                    dialog.Controls.Add(bigChart);
                    dialog.ShowDialog();
                };
            }
        }

        private Button GetSwitchButton()
        {
            var button = new Button();
            button.Click += (sender, ev) =>
            {
                switch (viewType)
                {
                    case SignalViewType.Signals:
                        viewSwitchButton.Text = "Суммарный";
                        viewType = SignalViewType.Sum;
                        Invalidate();
                        break;
                    case SignalViewType.Sum:
                        viewSwitchButton.Text = "Разделенный";
                        viewType = SignalViewType.Signals;
                        Invalidate();
                        break;
                }
                
                Controls.Remove(currentView);
                currentView = signalViewes[viewType];
                Controls.Add(currentView);
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            };

            return button;
        }

        private void Reconstruct()
        {
            signalViewes[SignalViewType.Signals] = new SignalChart(signalState.Signals, signalState.Size);
            signalViewes[SignalViewType.Sum] = new SignalChart(signalState.Sum);
        }
    }
}

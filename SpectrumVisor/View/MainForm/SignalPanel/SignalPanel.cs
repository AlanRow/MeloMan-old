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

    //отвечает за размещение информации о сигнале
    //содержит:
    /*
     * 1. панель изображения сигнала
     * 2. панель со списком сигналов
     */
    class SignalPanel : Panel
    {
        private ApplicationState state;

        static private Size minViewSize = new Size(200, 150);

        private SignalViewPanel view;
        private SignalsList list;

        public SignalPanel(ApplicationState state) : base()
        {
            view = new SignalViewPanel(state);
            list = new SignalsList(state);

            SizeChanged += (sender, ev) =>
            {
                view.SetBounds(0, 0, Height / 2, Height / 2);

                var listPrefs = list.PreferredSize;
                list.SetBounds(0, view.Height,
                                      listPrefs.Width, listPrefs.Height);
            };

            Controls.Add(view);
            Controls.Add(list);
            Invalidate();
        }

        //private void InitCharts()
        //{
        //    Reconstruct();
            
        //    foreach (var view in signalViewes.Values)
        //    {
        //        //увеличенный график
        //        var dialog = new Form();

        //        //отображается при клике по уменьшенному
        //        view.MouseClick += (obj, even) =>
        //        {
        //            var bigChart = new SignalChart(signalState.Signals, signalState.Size);
        //            bigChart.Width = 750;
        //            bigChart.Height = 750;
        //            dialog.Controls.Add(bigChart);
        //            dialog.ShowDialog();
        //        };
        //    }
        //}

        //private Button GetSwitchButton()
        //{
        //    var button = new Button();
        //    button.Click += (sender, ev) =>
        //    {
        //        switch (viewType)
        //        {
        //            case SignalViewType.Signals:
        //                viewSwitchButton.Text = "Суммарный";
        //                viewType = SignalViewType.Sum;
        //                Invalidate();
        //                break;
        //            case SignalViewType.Sum:
        //                viewSwitchButton.Text = "Разделенный";
        //                viewType = SignalViewType.Signals;
        //                Invalidate();
        //                break;
        //        }
                
        //        Controls.Remove(currentView);
        //        currentView = signalViewes[viewType];
        //        Controls.Add(currentView);
        //        OnSizeChanged(EventArgs.Empty);
        //        Invalidate();
        //    };

        //    return button;
        //}

        //private void Reconstruct()
        //{
        //    signalViewes[SignalViewType.Signals] = new SignalChart(signalState.Signals, signalState.Size);
        //    signalViewes[SignalViewType.Sum] = new SignalChart(signalState.Sum);
        //}
    }
}

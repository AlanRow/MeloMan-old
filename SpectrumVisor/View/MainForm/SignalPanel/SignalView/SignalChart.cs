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
    //выводит диаграму текущих сигналов
    class SignalChart : Chart
    {
        public SignalChart(ApplicationState state) : base()
        {
            var view = state.View.SignalView;
            //получаем текущие сигналы для вывода на экран
            var signalsOpts = view.GetCurrentViews().GetViewOptions();

            Titles.Add(view.GetCurrentName());
            ChartAreas.Add("signal");

            for (var i = 0; i < signalsOpts.Count; i++)
            {
                Series signalSeries = new Series(i.ToString());

                var signal = signalsOpts[i].Signal;
                signalSeries.ChartType = SeriesChartType.Line;
                signalSeries.ChartArea = "signal";
                var color = signalsOpts[i].Color;
                signalSeries.Color = color;

                var j = 0;
                foreach (var val in signal.GetValues())
                {
                    signalSeries.Points.AddXY(j, val);
                    j++;
                }
               
               Series.Add(signalSeries);
            }
        }

        //public SignalChart(double[] signal)
        //{
        //    Titles.Add("Summarized signal");
        //    ChartAreas.Add("sum");
        //    Series signalSeries = new Series("sum");

        //    for (var j = 0; j < signal.Length; j++)
        //    {
        //        signalSeries.Points.AddXY(j, signal[j]);
        //    }

        //    signalSeries.ChartType = SeriesChartType.Line;
        //    signalSeries.ChartArea = "sum";
        //    signalSeries.Color = Color.Red;
        //    Series.Add(signalSeries);
        //}
    }
}

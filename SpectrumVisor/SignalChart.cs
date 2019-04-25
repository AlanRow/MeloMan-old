using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SpectrumVisor
{
    class SignalChart : Chart
    {
        public SignalChart(List<SinGenerator> signals, int size)
        {
            var x = new double[size];
            
            for (var i = 0; i < signals.Count; i++)
            {
                ChartAreas.Add(i.ToString());
                Series signalSeries = new Series(i.ToString());
                signalSeries.Points.Add(signals[i].GenerateSin());
                signalSeries.ChartType = SeriesChartType.Line;
                signalSeries.ChartArea = i.ToString();
                Series.Add(signalSeries);
            }
        }

        public SignalChart(double[] signal)
        {
            ChartAreas.Add("signal");
            Series signalSeries = new Series("signal");
            signalSeries.Points.Add(signal);
            signalSeries.ChartType = SeriesChartType.Line;
            signalSeries.ChartArea = "signal";
            Series.Add(signalSeries);
        }
    }
}

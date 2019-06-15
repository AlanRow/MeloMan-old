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
    class SignalChart : Chart
    {
        public SignalChart(List<SinGenerator> signals, int size) : base()
        {
            Titles.Add("Signals");
            ChartAreas.Add("signal");

            for (var i = 0; i < signals.Count; i++)
            {
                Series signalSeries = new Series(i.ToString());

                var signal = signals[i].GenerateSin();
                signalSeries.ChartType = SeriesChartType.Line;
                signalSeries.ChartArea = "signal";
                var color = Color.FromArgb(255, i * 32 % 256, i * 64 % 256, i * 128 % 256);
                signalSeries.Color = color;

               for (var j = 0; j < signal.Length; j++)
               {
                   signalSeries.Points.AddXY(j, signal[j]);
               }
               
               Series.Add(signalSeries);
            }
        }

        public SignalChart(double[] signal)
        {
            Titles.Add("Summarized signal");
            ChartAreas.Add("sum");
            Series signalSeries = new Series("sum");

            for (var j = 0; j < signal.Length; j++)
            {
                signalSeries.Points.AddXY(j, signal[j]);
            }

            signalSeries.ChartType = SeriesChartType.Line;
            signalSeries.ChartArea = "sum";
            signalSeries.Color = Color.Red;
            Series.Add(signalSeries);
        }
    }
}

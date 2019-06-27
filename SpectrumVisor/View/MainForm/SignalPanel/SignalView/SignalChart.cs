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
    class SignalChart : Panel
    {
        private SignalViewState view;

        public SignalChart(SignalController signal) : base()
        {
            view = signal.View;
            var chart = GenerateChart();
            Controls.Add(chart);
            signal.ViewChanged += () => {
                Controls.Remove(chart);
                chart = GenerateChart();
                Controls.Add(chart);
            };
        }

        public Chart GenerateChart()
        {
            //получаем текущие сигналы для вывода на экран
            var signalsOpts = view.GetCurrentViews().GetViewOptions();

            var chart = new Chart();

            chart.Titles.Add(view.GetCurrentName());
            chart.ChartAreas.Add("signal");

            for (var i = 0; i < signalsOpts.Count; i++)
            {
                Series signalSeries = new Series(i.ToString());

                var signal = signalsOpts[i].Signal;
                signalSeries.ChartType = SeriesChartType.Line;
                signalSeries.ChartArea = "signal";
                var color = signalsOpts[i].Color;
                signalSeries.Color = color;

                var j = 0;
                var log = new Logger("view_values.1.txt");
                foreach (var val in signal.GetValues())
                {
                    signalSeries.Points.AddXY(j, val);
                    log.WriteLog(j + ": " + val);
                    j++;
                }

                chart.Series.Add(signalSeries);
            }

            return chart;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Drawing;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace SpectrumVisor
{
    static class Program
    {

        static double Sin(double t, double freqHz, double A, double phaseRad = 0d)
        {
            return A * Math.Sin(2 * Math.PI * freqHz * t + phaseRad);
        }

        public static void CreateSinSignal(double[] signal, int start, int size, int repeatsCount, double maxValue, double minValue)
        {
            for (int i = start; i < Math.Min(start + size, signal.Length) - 1; i++)
                signal[i] = Sin(i - start, 1.0/size*repeatsCount, (maxValue - minValue)/2) + (maxValue + minValue)/2;
        }

        [STAThread]
        static void Main()
        {
            /*Программа, которая рисует распределение центра "массы" при дискретном преобразовании Фурье*/

            int length = 256;

            //Изначальный сигнал
            var origin = new double[length];
            CreateSinSignal(origin, 0, length, 8, 11, 10);
            

            //распределение центра массы 

            Application.Run(new TransformViewer());
        }

    }

    public class MCConf
    {
        private double[] origin;
        public double W { get; set; }
        private int start;
        public int Start
        {
            get { return start; }
            set { start = value % origin.Length; }
        }
        public int WinSize { get; set; }
        public double Step { get; set; }

        public MCConf(double[] signal)
        {
            origin = signal;
            Start = 0;
            WinSize = signal.Length;
            W = Math.PI / 180;
        }

        //Find Mass Center
        public Complex FMC()
        {
            var fmc = Complex.Zero;

            for (var i = Start; i < Start + WinSize; i++)
                fmc += origin[i % origin.Length] * Complex.FromPolarCoordinates(1, -W * i * 2 * Math.PI);

            return fmc / WinSize;
        }
    }

    public class TransformViewer : Form
    {
        private double[] origin;

        private MCConf confs;
        private Complex MC;

        private Panel originPicture;
        private Panel MCPicture;

        public TransformViewer() : base()
        {
            //int length = 256;

            //Изначальный сигнал
            SetOrigin(256, 0, 4, 0, 1);

            //изоражение изначального сигнала
            originPicture = new Panel();
            originPicture.SetBounds(0, 0, 400, 400);
            RepaintOrigin();
            Controls.Add(originPicture);

            //настройки преобразования
            confs = new MCConf(origin);
            MC = confs.FMC();

            MCPicture = new Panel();
            MCPicture.SetBounds(450, 0, 700, 700);
            MCPicture.Paint += (sender, ev) =>
            {
                var gr = ev.Graphics;
                //var log = "points.txt";
                //File.Delete(log);

                gr.DrawEllipse(Pens.Cyan, 20, 20, MCPicture.Width - 20, MCPicture.Height - 20);
                var halfWidth = MCPicture.Width / 2 - 10;
                var halfHeight = MCPicture.Height / 2 - 10;

                var previous = new Point(halfWidth + 10, halfHeight + 10);
                for (var w = 0d; w < 1; w += 1.0/20)
                {
                    confs.W = w;
                    //File.AppendAllText(log, w.ToString() + "    " + confs.W.ToString() + "\n\r");
                    MC = confs.FMC();
                    var cmCoord = new Point(halfWidth + (int)Math.Round(MC.Real * halfWidth) + 10,
                                            halfHeight + (int)Math.Round(MC.Phase * halfHeight) + 10);
                    gr.FillEllipse(Brushes.Red, cmCoord.X - 5, cmCoord.Y - 5, 10, 10);
                    gr.DrawLine(Pens.Green, previous, cmCoord);
                    var wValue = new Label();
                    wValue.Text = (w*2).ToString() + "PI";
                    wValue.Location = new Point(cmCoord.X + 10, cmCoord.Y + 10);
                    MCPicture.Controls.Add(wValue);

                    previous = cmCoord;
                }

            };
            Controls.Add(MCPicture);

            var sizeTitle = new Label();
            sizeTitle.Location = new Point(50, 435);
            sizeTitle.Text = "Original array size:";
            Controls.Add(sizeTitle);

            var sizeBox = new TextBox();
            sizeBox.SetBounds(50, 460, 100, 30);
            sizeBox.Text = "256";
            Controls.Add(sizeBox);

            var repeatsTitle = new Label();
            repeatsTitle.Location = new Point(50, 500);
            repeatsTitle.Text = "Number of repeats:";
            Controls.Add(repeatsTitle);

            var repeatsBox = new TextBox();
            repeatsBox.SetBounds(50, 550, 100, 30);
            repeatsBox.Text = "4";
            Controls.Add(repeatsBox);

            var centerTitle = new Label();
            centerTitle.Location = new Point(200, 435);
            centerTitle.Text = "Y of center line:";
            Controls.Add(centerTitle);

            var centerBox = new TextBox();
            centerBox.SetBounds(200, 460, 100, 30);
            centerBox.Text = "0";
            Controls.Add(centerBox);

            var factorTitle = new Label();
            factorTitle.Location = new Point(200, 500);
            factorTitle.Text = "Factor of the Sin-function:";
            Controls.Add(factorTitle);

            var factorBox = new TextBox();
            factorBox.SetBounds(200, 550, 100, 30);
            factorBox.Text = "1";
            Controls.Add(factorBox);

            var setOriginButton = new Button();
            setOriginButton.SetBounds(100, 620, 100, 40);
            setOriginButton.Text = "Set signal";
            setOriginButton.Click += (sender, ev) =>
            {
                try
                {
                    var center = double.Parse(centerBox.Text);
                    var factor = double.Parse(factorBox.Text);
                    var min = center - factor / 2;
                    var max = center + factor / 2;

                    SetOrigin(int.Parse(sizeBox.Text), 0, int.Parse(repeatsBox.Text), min, max);
                    confs = new MCConf(origin);
                    //MCPicture.Controls
                    //SetOrigin(256, 0, 4, 0, 1);
                    RepaintOrigin();
                }
                catch (FormatException ex)
                {
                    setOriginButton.Text = "Some Error";
                }

                //originChart = BuildOriginChart();
                confs = new MCConf(origin);
                Invalidate();
            };
            Controls.Add(setOriginButton);
        }

        public void SetOrigin(int length, int start, int repeats, double min, double max)
        {
            origin = new double[length];
            Program.CreateSinSignal(origin, start, length, repeats, max, min);
        }

        public void RepaintOrigin()
        {
            var chartOrigin = BuildOriginChart();
            chartOrigin.SetBounds(0, 0, originPicture.Width, originPicture.Height);
            originPicture.Controls.Clear();
            originPicture.Controls.Add(chartOrigin);
        }

        private Chart BuildOriginChart()
        {

            var signalChart = new Chart();
            signalChart.Titles.Add("Origin");
            signalChart.Parent = this;
            signalChart.SetBounds(5, 0, Width / 2 - 5, Height / 2 - 5);
            signalChart.ChartAreas.Add("origin");

            Series signalSeries = new Series("origin");
            signalSeries.ChartType = SeriesChartType.Line;
            signalSeries.ChartArea = "origin";

            for (var j = 0; j < origin.Length; j++)
            {
                signalSeries.Points.AddXY(j, origin[j]);
            }

            signalChart.Series.Add(signalSeries);

            return signalChart;

        }
    }
}

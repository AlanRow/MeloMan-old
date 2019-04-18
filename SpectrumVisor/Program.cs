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

            int length = 1024;

            //Изначальный сигнал
            //var origin = new double[length];
            //CreateSinSignal(origin, 0, length, 8, 0, 100);

            //распределение центра массы 
            Application.Run(new TransformViewer());
        }

    }

    public class MCConf
    {
        private double[] origin;
        public double W { get; set; }//in grades
        public double WStep { get; set; }
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
            var norm = normalizeSignal(origin);

            for (var i = Start; i < Start + WinSize; i++)
                fmc += norm[i % norm.Length] * Complex.FromPolarCoordinates(1, -W * i / norm.Length * 2 * Math.PI);

            return fmc;
        }

        public Complex[] FMCs(int count, double freqStep)
        {
            var fmcs = new Complex[count];
            var norm = normalizeSignal(origin);
            File.Delete("norm.txt");
            File.AppendAllText("norm.txt", norm.Sum().ToString());

            var logs = new List<string>();

            for (var i = 1; i <= count; i++)
            {
                //logs.Add("i: " + i);
                var step = 1 / (i * freqStep);
                for (var j = Start; j < Start + WinSize; j++)
                {
                    fmcs[i - 1] += norm[j % norm.Length] * Complex.FromPolarCoordinates(1, -step * 2 * Math.PI * j);
                }

                //logs.Add("value: " + fmcs[i - 1]);
            }

            var logPath = "points2.txt";
            File.Delete(logPath);
            File.AppendAllLines(logPath, logs);

            return fmcs;
        }

        //после нормализации сумма элементов сигнала должна быть близка к 1
        public static double[] normalizeSignal(double[] signal)
        {
            var norm = new double[signal.Length]; 

            var sum = signal.Sum();//нужно переписать так, чтобы сохранять даже очень большие числа
            var av = sum / signal.Length;//среднее

            var avDev = 0d;//среднее отклонение от среднего
            for (var i = 0; i < signal.Length; i++)
            {
                var dev = Math.Abs(av - signal[i]);
                avDev *= i / (i + 1);
                avDev += dev / (i + 1);
            }

            var rising = - av + avDev;//смещение сигнала по высоте

            var avRis = 0d;//сумма со смещением

            for (var i = 0; i < signal.Length; i++)
            {
                avRis *= i / (i + 1);
                avRis += (signal[i] + rising) / (i + 1);
            }

            var normFactor = - 1 / (avRis * signal.Length * signal.Length);//множитель нормирования
            //теперь нормализуем сигнал
            for (var i = 0; i < signal.Length; i++)
            {
                norm[i] = (signal[i] + avRis) * normFactor;
            }

            return norm;
        }
    }

    public class TransformViewer : Form
    {
        private SinSignal origin;

        private MCConf confs;
        private Complex MC;

        private Panel originPicture;
        private Panel MCPicture;

        public TransformViewer() : base()
        {
            //Изначальный сигнал
            origin = new SinSignal(256, 32, 1, 1);

            //изоражение изначального сигнала
            originPicture = new Panel();
            originPicture.SetBounds(0, 0, 400, 400);
            RepaintOrigin();
            Controls.Add(originPicture);

            //настройки преобразования
            confs = new MCConf(origin.Signal);
            MC = confs.FMC();

            MCPicture = new Panel();
            MCPicture.SetBounds(450, 0, 700, 700);
            MCPicture.Paint += (sender, ev) =>
            {
                var gr = ev.Graphics;

                gr.DrawEllipse(Pens.Cyan, 20, 20, MCPicture.Width - 20, MCPicture.Height - 20);
                var halfWidth = MCPicture.Width / 2 - 10;
                var halfHeight = MCPicture.Height / 2 - 10;

                Tuple<float, float> previous = null;
                var logs = new List<string>();
                var centers = confs.FMCs(10, 1);

                foreach (var c in centers)
                {
                    logs.Add("magnitude: " + c.Magnitude + ",   complex: " + c.ToString());
                    var cmCoord = Tuple.Create((float)(halfWidth + halfWidth * c.Real), (float)(halfHeight + halfHeight * c.Imaginary));
                    gr.FillEllipse(Brushes.Red, cmCoord.Item1, cmCoord.Item2, 20, 20);
                    if (previous != null)
                        gr.DrawLine(Pens.Green, previous.Item1, previous.Item2, cmCoord.Item1, cmCoord.Item2);
                    previous = cmCoord;
                }

                //for (var w = 0d; w < 1; w += 1.0/20)
                //{
                //    confs.W = w;

                //    MC = confs.FMC();
                //    //logs.Add(w.ToString() + ",   magnitude: " + MC.Magnitude + ",   complex: " + MC.ToString());

                //    var cmCoord = new Point(halfWidth + (int)Math.Round(MC.Real * halfWidth) + 10,
                //                            halfHeight + (int)Math.Round(MC.Phase * halfHeight) + 10);
                //    gr.FillEllipse(Brushes.Red, cmCoord.X - 5, cmCoord.Y - 5, 10, 10);
                //    gr.DrawLine(Pens.Green, previous, cmCoord);
                //    var wValue = new Label();
                //    wValue.Text = (w*2).ToString() + "PI";
                //    wValue.Location = new Point(cmCoord.X + 10, cmCoord.Y + 10);
                //    MCPicture.Controls.Add(wValue);

                //    previous = cmCoord;
                //}

                var logPath = "points.txt";
                File.Delete(logPath);
                File.AppendAllLines(logPath, logs);

            };
            Controls.Add(MCPicture);

            InitOriginControllers();
        }

        private void InitOriginControllers()
        {
            var sizeTitle = new Label();
            sizeTitle.Location = new Point(50, 435);
            sizeTitle.Text = "Original array size:";
            Controls.Add(sizeTitle);

            var sizeBox = new TextBox();
            sizeBox.SetBounds(50, 460, 100, 30);
            sizeBox.Text = origin.Signal.Length.ToString();
            Controls.Add(sizeBox);

            var repeatsTitle = new Label();
            repeatsTitle.Location = new Point(50, 500);
            repeatsTitle.Text = "Number of repeats:";
            Controls.Add(repeatsTitle);

            var repeatsBox = new TextBox();
            repeatsBox.SetBounds(50, 550, 100, 30);
            repeatsBox.Text = origin.Repeats.ToString();
            Controls.Add(repeatsBox);

            var centerTitle = new Label();
            centerTitle.Location = new Point(200, 435);
            centerTitle.Text = "Y of center line:";
            Controls.Add(centerTitle);

            var centerBox = new TextBox();
            centerBox.SetBounds(200, 460, 100, 30);
            centerBox.Text = origin.AverageLine.ToString();
            Controls.Add(centerBox);

            var factorTitle = new Label();
            factorTitle.Location = new Point(200, 500);
            factorTitle.Text = "Factor of the Sin-function:";
            Controls.Add(factorTitle);

            var factorBox = new TextBox();
            factorBox.SetBounds(200, 550, 100, 30);
            factorBox.Text = origin.Deviance.ToString();
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

                    origin.SetSignal(int.Parse(sizeBox.Text), int.Parse(repeatsBox.Text), center, factor);
                    confs = new MCConf(origin.Signal);
                    //MCPicture.Controls
                    //SetOrigin(256, 0, 4, 0, 1);
                    RepaintOrigin();
                }
                catch (FormatException ex)
                {
                    setOriginButton.Text = "Some Error";
                }

                //originChart = BuildOriginChart();
                confs = new MCConf(origin.Signal);
                Invalidate();
            };
            Controls.Add(setOriginButton);
        }

        private void InitFTControllers()
        {
            var wTitle = new Label();
            wTitle.Location = new Point(400, 435);
            wTitle.Text = "Step of w:";
            Controls.Add(wTitle);

            var wBox = new TextBox();
            wBox.SetBounds(400, 460, 100, 30);
           // wBox.Text = origin.;
            //Controls.Add(sizeBox);

            var repeatsTitle = new Label();
            repeatsTitle.Location = new Point(50, 500);
            repeatsTitle.Text = "Number of repeats:";
            Controls.Add(repeatsTitle);

            var repeatsBox = new TextBox();
            repeatsBox.SetBounds(50, 550, 100, 30);
            repeatsBox.Text = origin.Repeats.ToString();
            Controls.Add(repeatsBox);

            var centerTitle = new Label();
            centerTitle.Location = new Point(200, 435);
            centerTitle.Text = "Y of center line:";
            Controls.Add(centerTitle);

            var centerBox = new TextBox();
            centerBox.SetBounds(200, 460, 100, 30);
            centerBox.Text = origin.AverageLine.ToString();
            Controls.Add(centerBox);

            var factorTitle = new Label();
            factorTitle.Location = new Point(200, 500);
            factorTitle.Text = "Factor of the Sin-function:";
            Controls.Add(factorTitle);

            var factorBox = new TextBox();
            factorBox.SetBounds(200, 550, 100, 30);
            factorBox.Text = origin.Deviance.ToString();
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

                    //origin.SetSignal(int.Parse(sizeBox.Text), int.Parse(repeatsBox.Text), center, factor);
                    confs = new MCConf(origin.Signal);
                    //MCPicture.Controls
                    //SetOrigin(256, 0, 4, 0, 1);
                    RepaintOrigin();
                }
                catch (FormatException ex)
                {
                    setOriginButton.Text = "Some Error";
                }

                //originChart = BuildOriginChart();
                confs = new MCConf(origin.Signal);
                Invalidate();
            };
            Controls.Add(setOriginButton);
        }


        private void RepaintOrigin()
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

            for (var j = 0; j < origin.Signal.Length; j++)
            {
                signalSeries.Points.AddXY(j, origin.Signal[j]);
            }

            signalChart.Series.Add(signalSeries);

            return signalChart;

        }
    }

    public class SinSignal
    {
        public double[] Signal { get; private set; }
        private double startAngle = 0;
        public int Repeats { get; private set; }
        public double AverageLine { get; private set; }
        public double Deviance { get; private set; }

        public SinSignal(int length, int reps, double av, double dev)
        {
            Repeats = reps;
            AverageLine = av;
            Deviance = dev;

            SetSignal(length, Repeats, AverageLine, Deviance);
        }


        public void SetSignal(int length, int repeats, double avLine, double dev)
        {
            Signal = new double[length];
            dev = Math.Abs(dev);
            Repeats = repeats;
            AverageLine = avLine;
            Deviance = dev;
            Program.CreateSinSignal(Signal, 0, length, repeats, avLine + dev, avLine - dev);
        }
    }
}

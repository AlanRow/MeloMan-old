using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{

    class RoundSpectrum : Panel
    {
        private RoundOptions opts;
        private static int WHEEL_DELTA = 120;

        public RoundSpectrum(RoundOptions options)
        {
            opts = options;
            DoubleBuffered = true;

            //фишка для увеличения графика при помощи скролла. Пока не работает.
            MouseClick += (sender, ev) =>
            {
                opts.ZoomScale(1.1);
                var log = new Logger("zoom.txt");
                log.WriteLog(opts.ScalePercents.ToString());
                log.Flush();
            };

            MouseWheel += (sender, ev) =>
            {
                opts.ZoomScale(1.1);
                var log = new Logger("zoom.txt");
                log.WriteLog(opts.ScalePercents.ToString());
                log.Flush();
                //opts.ZoomScale(Math.Pow(1.1, ev.Delta / WHEEL_DELTA));
            };

            Invalidate();
        } 

        protected override void OnPaint(PaintEventArgs args)
        {
            var bitmapChart = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            //var gr = args.Graphics;
            var scale = opts.ScalePercents / 100;
            var size = (int)Math.Round(Math.Min(Width * scale, Height * scale));
            var gr = Graphics.FromImage(bitmapChart);

            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

            gr.Clear(Color.White);

            var circlePen = new Pen(opts.CircleColor);
            gr.DrawEllipse(circlePen, opts.CircleThickness, opts.CircleThickness, size, size);
            Point? last = null;

            foreach (var freq in opts.Points())
            {
                var value = freq.Coords;
                if (double.IsNaN(value.Real) || double.IsNaN(value.Imaginary))
                    break;
                
                Point? current = new Point((int)Math.Round((value.Real * size + size) / 2), (int)Math.Round((value.Imaginary * size + size) / 2));

                var pointBr = new SolidBrush(opts.PointColor);
                var rad = opts.PointRadius;
                gr.FillEllipse(pointBr, current.Value.X - rad, current.Value.Y - rad, 2 *rad, 2 * rad);

                if (last != null)
                    gr.DrawLine(Pens.Orange, last.Value, current.Value);
                
                gr.DrawString(freq.Freq.ToString(), opts.TextFont, Brushes.Black, current.Value.X - rad,
                    current.Value.Y - rad);

                last = current;
            }

            args.Graphics.DrawImage(bitmapChart, ClientRectangle);
        }
    }
}

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

    class RoundSpectrum : SpectrumView
    {
        private RoundOptions opts;
        private static int WHEEL_DELTA = 120;

        public RoundSpectrum(RoundOptions options)
        {
            opts = options;
            DoubleBuffered = true;

            //если в спектре только один вариант положения окна, то не имеет смысла показывать трекер
            if (opts.SpecSize > 1) {
                var tracker = new WindowTracker(opts);
                tracker.Dock = DockStyle.Bottom;
                tracker.Invalidated += (sender, ev) => { Invalidate(); };
                Controls.Add(tracker);
            }

            //фишка для увеличения графика при помощи скролла. Пока не работает.
            MouseWheel += (sender, ev) =>
            {
                opts.ZoomScale(1.1);
                var log = new Logger("zoom.txt");
                log.WriteLog(opts.ScalePercents.ToString());
                log.Flush();
            };

            Invalidate();
        } 

        //реализовать метод безопасно
        public override void Update(FreqPoint[][] newSpec)
        {
                opts.UpdateSpectrum(newSpec);
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            var bitmapChart = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            var scale = opts.ScalePercents / 100;
            var size = (int)Math.Round(Math.Min(Width * scale, Height * scale));
            var gr = Graphics.FromImage(bitmapChart);

            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

            gr.Clear(Color.White);

            var circlePen = new Pen(opts.CircleColor);
            gr.DrawEllipse(circlePen, opts.CircleThickness, opts.CircleThickness, size, size);
            Point? last = null;

            var firstPoint = true;//log
            foreach (var freq in opts.Points())
            {
                //log
                //if (firstPoint)
                //{
                //    Logger.DEFLOG.WriteLog(String.Format("This was invoked with first point is {0}", freq.Coords.Magnitude));
                //    Logger.DEFLOG.Flush();
                //    firstPoint = false;
                //}

                var value = freq.Coords;
                if (double.IsNaN(value.Real) || double.IsNaN(value.Imaginary))
                    break;
                
                Point? current = new Point((int)Math.Round((value.Real * size + size) / 2), 
                    (int)Math.Round((value.Imaginary * size + size) / 2));

                var pointBr = new SolidBrush(opts.PointColor);
                var rad = opts.PointRadius;
                gr.FillEllipse(pointBr, current.Value.X - rad, current.Value.Y - rad, 2 *rad, 2 * rad);

                if (last != null)
                    gr.DrawLine(Pens.Orange, last.Value, current.Value);

                var freqStr = freq.Freq.ToString();
                gr.DrawString(freqStr, opts.TextFont, Brushes.Black, current.Value.X - rad,
                    current.Value.Y - rad);

                last = current;
            }

            

            args.Graphics.DrawImage(bitmapChart, ClientRectangle);
        }
    }
}

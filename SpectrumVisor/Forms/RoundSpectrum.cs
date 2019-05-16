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

    class RoundSpectrum : PictureBox
    {
        private Complex[][] spectrum;

        public RoundSpectrum(Complex[][] complexes)
        {
            spectrum = complexes;
            Width = 400;
            Height = 400;
        } 

        protected override void OnPaint(PaintEventArgs args)
        {
            var gr = args.Graphics;
            var size = Math.Min(Width, Height);


            Width = 400;
            Height = 400;

            var log = new Logger("round_size.txt");
            log.WriteLog("Width: " + Width);
            log.WriteLog("Height: " + Height);
            log.WriteLog("Size: " + size);
            log.Flush();

            gr.DrawEllipse(Pens.Red, 0, 0, size, size);
            Point? last = null;

            foreach (var value in spectrum[0])
            {
                Point? current = new Point((int)Math.Round(value.Real + (size / 2)), (int)Math.Round(value.Imaginary + (size / 2)));
                gr.FillEllipse(Brushes.DarkBlue, current.Value.X, current.Value.X, 2, 2);

                if (last != null)
                    gr.DrawLine(Pens.Orange, last.Value, current.Value);

                last = current;
            }
        }
    }
}

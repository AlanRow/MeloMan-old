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
        } 

        protected override void OnPaint(PaintEventArgs args)
        {
            var gr = args.Graphics;
            var size = Math.Min(Width, Height);

            gr.Clear(Color.White);
            gr.DrawEllipse(Pens.Red, 5, 5, size - 5, size - 5);
            Point? last = null;

            for (var i = 0; i < spectrum[0].Length; i++)
            {
                var value = spectrum[0][i];
                if (double.IsNaN(value.Real) || double.IsNaN(value.Imaginary))
                    break;

                Point? current = new Point((int)Math.Round((value.Real * size + size) / 2), (int)Math.Round((value.Imaginary * size + size) / 2));
                gr.FillEllipse(Brushes.DarkBlue, current.Value.X, current.Value.Y, 5, 5);

                var number = new Label
                {
                    Text = i.ToString(),
                    Location = current.Value,
                    Width = 10
                };
                Controls.Add(number);

                if (last != null)
                    gr.DrawLine(Pens.Orange, last.Value, current.Value);

                last = current;
            }
        }
    }
}

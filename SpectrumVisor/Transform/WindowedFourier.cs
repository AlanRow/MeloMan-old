using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class WindowedFourier : FourierTransformer, IWindowedTransformer
    {
        public readonly int WinSize;
        private int start;

        public WindowedFourier(int winSize) : base()
        {
            WinSize = winSize;
            start = -WinSize / 2;
        }

        protected override Complex findMC(double[] signal, double w)
        {
            var mc = Complex.Zero;

            for (var i = start; i < WinSize / 2; i++)
            {
                var ind = (mod(i + WinSize, signal.Length));
                mc += signal[ind] * Complex.FromPolarCoordinates(1, -w * i / signal.Length * 2 * Math.PI);
            }

            return mc;
        }

        public override Complex[][] GetSpectrum(double[] signal, int stepsCount, double start, double step)
        {
            var spectrum = new Complex[signal.Length][];

            for (var i = 0; i < spectrum.Length; i++)
            {
                start++;
                spectrum[i] = Transform(signal, stepsCount, start, step);
            }

            return spectrum;
        }

        private static int mod(int number, int basis)
        {
            var m = number % basis;

            return (m >= 0) ? m : basis + m;
        }

        public double GetWinSize()
        {
            return WinSize;
        }
    }
}

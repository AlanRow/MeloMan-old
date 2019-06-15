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
        protected WindowedNormalizer normalizer;
        private int start;

        public WindowedFourier(int winSize)
        {
            WinSize = winSize;
            start = -WinSize / 2;
            normalizer = new WindowedNormalizer(WinSize);
        }

        protected override Complex findMC(double[] signal, double w)
        {
            var mc = Complex.Zero;

            for (var i = start; i < start + WinSize; i++)
            {
                var ind = (mod(i, signal.Length));
                mc += signal[ind] * Complex.FromPolarCoordinates(1, -w * i / WinSize * 2 * Math.PI);
            }

            return mc;
        }
        //public override FreqPoint[] Transform(double[] signal, int stepsCount, double wStart, double wStep) {
        //    return base.Transform(normalizer.Norm(signal), stepsCount, wStart, wStep);
        //}

        public override FreqPoint[][] GetSpectrum(double[] signal, int stepsCount, double wStart, double step)
        {
            var spectrum = new FreqPoint[signal.Length][];
            start = 0;

            for (var i = 0; i < spectrum.Length; i++)
            {
                spectrum[i] = Transform(GetWindow(signal), stepsCount, wStart, step);
                start++;
            }

            return spectrum;
        }

        private static int mod(int number, int basis)
        {
            var m = number % basis;

            return (m >= 0) ? m : basis + m;
        }

        private double[] GetWindow(double[] signal)
        {
            var win = new double[signal.Length];

            for (var i = 0; i < win.Length; i++)
                win[i] = (i >= start && i < start + WinSize) ? signal[i] : 0;

            return win;
        }

        public double GetWinSize()
        {
            return WinSize;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class WindowedFourier : FourierTransformer
    {
        public readonly int WinSize;
        private int start;

        public WindowedFourier(double wStart, double wStep, int stepsCount, int winSize) : base(wStart, wStep, stepsCount)
        {
            WinSize = winSize;
            start = -WinSize / 2;
        }

        protected override Complex findMC(double[] signal)
        {
            var mc = Complex.Zero;

            for (var i = 0; i < WinSize; i++)
                mc += signal[mod((i + start), signal.Length)] * Complex.FromPolarCoordinates(1, -WStart * i / signal.Length * 2 * Math.PI);

            return mc;
        }

        public override Complex[][] GetSpectrum(double[] signal)
        {
            var spectrum = new Complex[signal.Length][];

            for (var i = 0; i < spectrum.Length; i++)
            {
                start++;
                spectrum[i] = Transform(signal);
            }

            return spectrum;
        }

        private static int mod(int number, int basis)
        {
            var m = number % basis;

            return (m >= 0) ? m : basis + m;
        }

    }
}

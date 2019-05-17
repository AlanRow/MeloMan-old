using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class FourierTransformer : ITransformer
    {
        virtual protected Complex findMC(double[] signal, double w)
        {
            var mc = Complex.Zero;

            for (var i = 0; i < signal.Length; i++)
                mc += signal[i] * Complex.FromPolarCoordinates(1, -w * i / signal.Length * 2 * Math.PI);

            return mc;
        }

        public Complex[] Transform(double[] signal, int stepsCount, double wStart, double wStep)
        {
            var tr = new Complex[stepsCount];

            for (var i = 0; i < stepsCount; i++)
            {
                var step = wStart + i * wStep;//1 / (wStart + i * wStep);
                tr[i] = findMC(signal, step);
            }

            return tr;
        }

        virtual public Complex[][] GetSpectrum(double[] signal, int stepsCount, double start, double step)
        {
            var spectrum = new Complex[signal.Length][];
            var tr = Transform(signal, stepsCount, start, step);

            for (var i = 0; i < spectrum.Length; i++)
            {
                spectrum[i] = tr;
            }
            
            return spectrum;
        }

    }
}

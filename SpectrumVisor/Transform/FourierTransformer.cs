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
        protected SignalNormalizer norm;

        public FourierTransformer()
        {
            norm = new SignalNormalizer();
        }

        virtual protected Complex findMC(double[] signal, double w)
        {
            var mc = Complex.Zero;

            for (var i = 0; i < signal.Length; i++)
                mc += signal[i] * Complex.FromPolarCoordinates(1, -w * i / signal.Length * 2 * Math.PI);

            return mc;
        }

        virtual public FreqPoint[] Transform(double[] signal, int stepsCount, double wStart, double wStep)
        {
            var tr = new FreqPoint[stepsCount];
            var normSignal = norm.Norm(signal);
            var step = wStart;

            for (var i = 0; i < stepsCount; i++)
            {
                tr[i] = new FreqPoint(findMC(normSignal, step), step);
                step += wStep;
            }

            return tr;
        }

        virtual public FreqPoint[][] GetSpectrum(double[] signal, int stepsCount, double start, double step)
        {
            var spectrum = new FreqPoint[][] { Transform(signal, stepsCount, start, step) };
            
            return spectrum;
        }

    }
}

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
        public double WStart { get; private set; }//in grades
        public double WStep { get; private set; }
        public int StepsCount { get; private set; }

        public FourierTransformer(double start, double step, int stepsCount)
        {
            WStart = start;
            WStep = step;
            StepsCount = stepsCount;
        }

        public FourierTransformer(): this(1, 1, 8){ }

        //Find Mass Center
        virtual protected Complex findMC(double[] signal)
        {
            var mc = Complex.Zero;

            for (var i = 0; i < signal.Length; i++)
                mc += signal[i] * Complex.FromPolarCoordinates(1, -WStart * i / signal.Length * 2 * Math.PI);

            return mc;
        }

        public Complex[] Transform(double[] signal)
        {
            var tr = new Complex[StepsCount];

            for (var i = 0; i < StepsCount; i++)
            {
                var step = 1 / (WStart + i * WStep);
                tr[i] = findMC(signal);
            }
            
            return tr;
        }

        virtual public Complex[][] GetSpectrum(double[] signal)
        {
            var spectrum = new Complex[signal.Length][];
            var tr = Transform(signal);

            for (var i = 0; i < spectrum.Length; i++)
            {
                spectrum[i] = tr;
            }

            return spectrum;
        }

    }
}

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
        //public double WStart { get; private set; }//in grades
        //public double WStep { get; private set; }
        //public int StepsCount { get; private set; }

        public FourierTransformer(/*double start, double step, int stepsCount*/)
        {
            //WStart = start;
            //WStep = step;
            //StepsCount = stepsCount;
        }

        //public FourierTransformer(): this(1, 1, 8){ }

        //Find Mass Center
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
                var step = 1 / (wStart + i * wStep);
                tr[i] = findMC(signal, step);
            }
            
            
            var logger = new Logger("tr2.txt");
            foreach (var v in tr)
                logger.WriteLog(v.ToString());
            logger.Flush();

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

            var logger = new Logger("tr1.txt");
            foreach (var v in tr)
                logger.WriteLog(v.Magnitude.ToString());
            logger.Flush();

            return spectrum;
        }

    }
}

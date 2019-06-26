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
        private SignalManager signal;
        protected SignalNormalizer norm;
        protected StdOptions options;
        protected FreqPoint[][] spectrum;

        public FourierTransformer(SignalManager manager)
        {
            signal = manager;
            norm = new SignalNormalizer();
            options = new StdOptions();
            spectrum = new FreqPoint[0][];
            CalcSpectrum();
        }

        virtual public void SwitchOptions(StdOptions newOptions)
        {
            options = new StdOptions();
        }

        public IEnumerable<double> GetFreqs()
        {
            var freq = options.StartFreq;

            for (var i = 0; i < options.CountFreq; i++)
            {
                yield return freq;
                freq += options.StepFreq;
            }
        }

        virtual protected Complex findMC(double[] signal, double w)
        {
            var mc = Complex.Zero;

            for (var i = 0; i < signal.Length; i++)
                mc += signal[i] * Complex.FromPolarCoordinates(1, -w * i / signal.Length * 2 * Math.PI);

            return mc;
        }

        virtual public FreqPoint[] Transform(IEnumerable<double> signal)
        {
            var tr = new FreqPoint[options.CountFreq];
            var normSignal = norm.Norm(signal.ToArray());
            var step = options.StartFreq;

            for (var i = 0; i < options.CountFreq; i++)
            {
                tr[i] = new FreqPoint(findMC(normSignal, step), step);
                step += options.StepFreq;
            }

            return tr;
        }

        virtual public async void CalcSpectrum()
        {
            var spec = new FreqPoint[1][];
            var signalArray = signal.Sum.GetValues();
            await Task.Run(() => {
                spec[0] = Transform(signalArray);
            });

            lock (spectrum)
            {
                spectrum = spec;
            }
        }

        virtual public FreqPoint[][] GetSpectrum()
        {
            lock (spectrum)
            {
                return spectrum;
            }
        }

    }
}

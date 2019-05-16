using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class TransformManager
    {
        private ITransformer transformer;
        private Complex[][] spectrum;
        private SignalNormalizer norm;

        public int FreqSize { get; private set; }
        public double StartFreq { get; private set;}
        public double FreqStep { get; private set; }
        public double WndowSize { get; private set; }

        public delegate void SpectrumChanged();
        public event SpectrumChanged Retransformed;

        SignalManager signal;

        
        public TransformManager(ITransformer transform, SignalManager sign)
        {
            transformer = transform;
            signal = sign;
            norm = new SignalNormalizer();

            FreqSize = 16;
            StartFreq = 1;
            FreqStep = 1;

            sign.AddedSignal += (values) =>
            {
                UpdateSpectrum();
            };

            sign.DeletedSignal += (values) =>
            {
                UpdateSpectrum();
            };

            spectrum = new Complex[0][];
            UpdateSpectrum();
        }

        public void SetParams(int freqs, double step, double start)
        {
            FreqSize = freqs;
            FreqStep = step;
            StartFreq = start;

            UpdateSpectrum();
        }

        private void UpdateSpectrum()
        {
            var normSignal = norm.Norm(signal.Sum);

            lock (spectrum)
            {
                spectrum = transformer.GetSpectrum(normSignal, FreqSize, FreqStep, StartFreq);
            }

            if (Retransformed != null)
                Retransformed();
        }

        public Complex[][] GetSpectrum()
        {
            lock (spectrum)
            {
                return spectrum;
            }
        }
    }
}

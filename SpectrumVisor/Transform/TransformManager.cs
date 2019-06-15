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
        private FreqPoint[][] spectrum;
        private SignalNormalizer norm;

        public int FreqSize { get; private set; }
        public double StartFreq { get; private set;}
        public double FreqStep { get; private set; }
        //public double WndowSize { get; private set; }

        public delegate void SpectrumChanged();
        public event SpectrumChanged Retransformed;

        SignalManager signal;

        
        public bool HaveWindow
        {
            get
            {
                return transformer is IWindowedTransformer;
            }
        }

        public double WinSize
        {
            get
            {
                return (transformer as IWindowedTransformer).GetWinSize();
            }
        }

        public double[] GetFreqsValues()
        {
            var freqs = new double[FreqSize];
            var f = StartFreq;

            for (var i = 0; i < FreqSize; i++)
            {
                freqs[i] = f;
                f += FreqStep;
            }

            return freqs;
        }

        public TransformManager(ITransformer transform, SignalManager sign)
        {
            transformer = transform;
            signal = sign;
            norm = new SignalNormalizer();

            FreqSize = 16;
            StartFreq = 1;
            FreqStep = 1;

            sign.AddedSignal += (values, index) =>
            {
                UpdateSpectrum();
            };

            sign.DeletedSignal += (values, index) =>
            {
                UpdateSpectrum();
            };

            spectrum = new FreqPoint[0][];
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
            //var normSignal = norm.Norm(signal.Sum);

            lock (spectrum)
            {
                spectrum = transformer.GetSpectrum(signal.Sum, FreqSize, FreqStep, StartFreq);
            }

            if (Retransformed != null)
                Retransformed();
        }

        public FreqPoint[][] GetSpectrum()
        {
            lock (spectrum)
            {
                return spectrum;
            }
        }
    }
}

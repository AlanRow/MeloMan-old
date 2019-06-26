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
        private Dictionary<TransformType, ITransformer> transformers;
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
            return transformer.GetFreqs().ToArray();
        }

        public TransformManager(SignalManager sign)
        {
            transformers = new Dictionary<TransformType, ITransformer>
            {
                [TransformType.Fourier] = new FourierTransformer(sign),
                //[TransformType.Windowed] = new WindowedFourier()
            };

            transformer = transformers[TransformType.Fourier];
            signal = sign;
            norm = new SignalNormalizer();

            spectrum = new FreqPoint[0][];
            UpdateSpectrum();
        }

        public void SwitchTransform(TransformType type)
        {
            transformer = transformers[type];
        }

        public void SetParams(StdOptions newOpts)
        {
            UpdateSpectrum();
        }

        public void UpdateSpectrum()
        {
            transformer.CalcSpectrum();
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

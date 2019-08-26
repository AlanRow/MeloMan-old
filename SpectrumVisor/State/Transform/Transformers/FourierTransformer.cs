using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //позволяет при изменениях в сигнале асинхронно вычислить и закэшировать преобразование
    class CachTransformer : ITransformer
    {
        //protected SignalNormalizer norm;
        //public StdOptions Options { get; private set; }

        //private ISignal signal;
        //private IFreqCalculator calculator;
        //private FreqPoint[][] spectrum;

        private ISpecGenerator generator;
        private FreqPoint[][] spectrum; 

        public CachTransformer(ISpecGenerator gener)
        {
            spectrum = new FreqPoint[0][];

        }

        public IEnumerable<double> GetFreqs()
        {
            throw new NotImplementedException();
        }

        public void SwitchOptions(StdOptions newOptions)
        {
            generator.SwitchOptions(newOptions);
        }

        FreqPoint[][] ITransformer.GetSpectrum()
        {
            //синхронизация, чтобы не возникло проблем при пересчете спктра
            lock (spectrum)
            {
                return spectrum;
            }
        }

        //public CachTransformer(ISignal origin, IFreqCalculator calc)
        //{
        //    signal = origin;
        //    //norm = new SignalNormalizer();
        //    Options = new StdOptions();
        //    spectrum = new FreqPoint[0][];
        //    CalcSpectrum();
        //}

        //virtual public void SwitchOptions(StdOptions newOptions)
        //{
        //    Options = new StdOptions();
        //}

        //public IEnumerable<double> GetFreqs()
        //{
        //    var freq = Options.StartFreq;

        //    for (var i = 0; i < Options.CountFreq; i++)
        //    {
        //        yield return freq;
        //        freq += Options.StepFreq;
        //    }
        //}

        //virtual public async void CalcSpectrum()
        //{
        //    var spec = new FreqPoint[1][];
        //    var signalArray = signal.Sum.GetValues();
        //    await Task.Run(() => {
        //        spec[0] = Transform(signalArray);
        //    });

        //    lock (spectrum)
        //    {
        //        spectrum = spec;
        //    }
        //}

        //virtual public FreqPoint[][] GetSpectrum()
        //{
        //    lock (spectrum)
        //    {
        //        return spectrum;
        //    }
        //}

    }
}

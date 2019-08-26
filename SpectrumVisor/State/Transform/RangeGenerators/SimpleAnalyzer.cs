using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //простая реализация пребразования без окна
    class SimpleAnalyzer : ISpectrumAnalyzer
    {
        private ISignal signal;
        private FourierCalculator calculator;

        public SimpleAnalyzer(ISignal origin)
        {
            signal = origin;
            calculator = new FourierCalculator();
        }

        public FreqPoint[] GetRange(StdOptions options)
        {
            var range = new FreqPoint[options.CountFreq];
            var w = options.StartFreq;
            for (var i = 0; i < options.CountFreq; i++)
            {
                range[i] = new FreqPoint(calculator.CalcFreq(signal, w), w);
                w += options.StepFreq;
            }

            return range;
        }
    }
}

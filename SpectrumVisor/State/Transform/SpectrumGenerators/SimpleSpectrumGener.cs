using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //расчитывает одиночный (по причине отсутствия окна) массив спектра
    class SimpleSpectrumGener : ISpecGenerator
    {
        private SimpleAnalyzer analyzer;
        private ISignal signal;
        public StdOptions Options { get; private set; }

        public SimpleSpectrumGener(ISignal origin, StdOptions opts)
        {
            analyzer = new SimpleAnalyzer(origin);
            Options = opts;
        }

        public SimpleSpectrumGener(ISignal origin) : this(origin, new StdOptions()) { }

        public FreqPoint[][] GetSpectrum()
        {
            return new FreqPoint[][] { analyzer.GetRange(Options) };
        }

        public void SwitchOptions(StdOptions opts)
        {
            Options = opts;
        }
    }
}

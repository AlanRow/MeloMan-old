using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //класс, отвечающий за динамическую генерацию сигнала
    class SinSignal : ISignal
    {
        public readonly SignalOptions Options;

        public SinSignal(SignalOptions options)
        {
            Options = options;
        }

        public int GetLength()
        {
            return Options.Duration;
        }

        public double GetValueAt(int i)
        {
            return (i < Options.Start || i >= Options.Start + Options.Duration) ? 0 :
                    Math.Sin((i + Options.PhaseOffset) * Options.Freq * Math.PI) * Options.Mult + Options.Const;
        }

        virtual public IEnumerable<double> GetValues()
        {
            for (var i = Options.Start; i < Options.Start + Options.Duration; i++)
            {
                yield return GetValueAt(i);
            }
        }
    }
}

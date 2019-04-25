using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class SignalManager
    {
        public List<SinGenerator> Signals{ get; private set; }
        public double[] Sum { get; private set; }
        public double TimeDuration { get; private set; }
        public int Size { get; private set; }

        public SignalManager(double sec, int size)
        {
            Signals = new List<SinGenerator>();
            Sum = new double[size];
            TimeDuration = sec;
            Size = size;
        }

        public SignalManager(double sec, double pointInSec) : this(sec, (int)Math.Floor(sec * pointInSec))
        { }

        public double[] AddSignalBySize(int start, int dur, double offset, double freq, double mult, double constant, double fading)
        {
            var gen = new SinGenerator(start, dur, offset, freq, mult, constant, fading);
            var signal = gen.GenerateSin();

            for (var i = 0; i < Math.Min(Size - start, dur); i++)
            {
                Sum[i + start] += signal[i];
            }

            Signals.Add(gen);
            return signal;
        }

        public void DeleteSignal(int index)
        {
            Signals.RemoveAt(index);
            ReCalcSum();
        }

        private void ReCalcSum()
        {
            foreach (var gen in Signals)
            {
                var signal = gen.GenerateSin();
                for (var i = 0; i < Math.Min(Size - Math.Ceiling(gen.Start), gen.Duration); i++)
                {
                    Sum[i + (int)Math.Ceiling(gen.Start)] += signal[i];
                }
            }
        }

    }
}

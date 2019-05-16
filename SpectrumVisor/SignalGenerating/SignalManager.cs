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

        public delegate void SignalsChanged(SinGenerator gener, int index);
        public event SignalsChanged AddedSignal;
        public event SignalsChanged DeletedSignal;

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

            for (var i = 0; i < signal.Length; i++)
                Sum[(start + i) % Sum.Length] += signal[i]; 

            Signals.Add(gen);

            if (AddedSignal != null)
                AddedSignal(gen, Signals.Count - 1);

            return signal;
        }

        public void DeleteSignal(int index)
        {
            var deleting = Signals[index];
            Signals.RemoveAt(index);
            ReCalcSum();

            if (DeletedSignal != null)
            {
                DeletedSignal(deleting, index);
            }
        }

        private void ReCalcSum()
        {
            var newSum = new double[Sum.Length];
            foreach (var gen in Signals)
            {
                var signal = gen.GenerateSin();
                for (var i = 0; i < Math.Min(Size - Math.Ceiling(gen.Start), gen.Duration); i++)
                {
                    newSum[i + (int)Math.Ceiling(gen.Start) % Sum.Length] += signal[i];
                }
            }

            Sum = newSum;
        }

    }
}

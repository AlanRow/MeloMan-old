using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class WindowedNormalizer : SignalNormalizer
    {
        public int WinSize { get; private set; }
        private int currentCenter;

        public WindowedNormalizer(int windowSize) : base()
        {
            WinSize = windowSize;
            currentCenter = 0; 
        }

        public override double[] Norm(double[] signal)
        {
            return base.Norm(getWindow(signal, currentCenter - WinSize / 2, WinSize).ToArray());
        }

        public void ShiftForward()
        {
            currentCenter++;
        }

        public void Reset()
        {
            currentCenter = 0;
        }

        private IEnumerable<double> getWindow(double[] signal, int start, int length)
        {
            for (var i = start; i < start + length; i++)
            {
                var ind = (i < 0) ? signal.Length + (i % signal.Length) : i % signal.Length;
                yield return signal[ind];
            }
        }
    }
}

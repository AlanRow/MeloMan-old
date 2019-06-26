using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor.State.Signal
{
    class DigitalSignal : ISignal
    {
        private double[] signal;

        public DigitalSignal(double[] values)
        {
            signal = values;
        }

        public IEnumerable<double> GetValues()
        {
            return signal;
        }
    }
}

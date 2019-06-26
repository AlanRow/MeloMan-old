using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class InState
    {
        public SignalManager Signal { get; private set; }
        private TransformManager transformer;

        public InState()
        {
            var signal = new SignalManager(1024);
            signal.AddSignal(new SinSignal(new SignalOptions(0, 1024)));

            transformer = new TransformManager(signal);
        }
    }
}

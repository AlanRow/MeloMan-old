using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class ManagerVisualizer : ISignalsVisualizer
    {
        private List<SignalViewOptions> options;

        public ManagerVisualizer(SignalManager manager)
        {
            options = new List<SignalViewOptions>();

            options.AddRange(manager.Signals.Select((s) => new SignalViewOptions(s)));

            //manager.AddedSignal += (signal) => { options.Add(new SignalViewOptions(signal)); };
            //manager.DeletedSignal += (signal) => {
            //    for (var i = 0; i < options.Count; i++)
            //        if (options[i].Signal.Equals(signal))
            //        {
            //            options.RemoveAt(i);
            //            break;
            //        }
            //};
        }

        public void AddSignal(SinSignal signal)
        {
            options.Add(new SignalViewOptions(signal));
        }

        public List<SignalViewOptions> GetViewOptions()
        {
            return options;
        }
    }
}

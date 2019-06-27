using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //реализует интерфейс для графика с единственным сигналом
    class AloneVisualizer : ISignalsVisualizer
    {
        private new List<SignalViewOptions> alone;

        public AloneVisualizer(ISignal signal)
        {
            alone = new List<SignalViewOptions>();
            alone.Add(new SignalViewOptions(signal, Color.Yellow));
        }

        //не будет никакого эффекта, т.к. в одиночный сигнал нельзя добавлять сигналы
        public void AddSignal(SinSignal signal)
        {
            return;
        }

        public List<SignalViewOptions> GetViewOptions()
        {
            return alone;
        }


    }
}

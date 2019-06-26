using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //предоставляет информацию об особенностях отрисовки данного сигнала
    //WORK
    //COMPLETE
    class SignalViewOptions
    {
        //возможно лучше по другому (используя Signal)
        readonly public ISignal Signal;
        //readonly public SignalOptions Unview;
        public Color Color { get; private set; }

        public SignalViewOptions(ISignal signal, Color color)
        {
            Signal = signal;
            //Unview = opts;
            Color = color;
        }

        //public SignalViewOptions(SinSignal signal, Color color) : this(signal, signal.Options, color){}

        public SignalViewOptions(ISignal signal) : this(signal, Color.Black) { }
    }
}

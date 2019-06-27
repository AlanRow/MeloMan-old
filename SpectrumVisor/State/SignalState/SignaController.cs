using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //объединяет визуальную и внутреннюю информацию о сигнале
    class SignalController
    {
        public static int DEFAULT_SIZE = 1024;

        public readonly SignalManager Internal;
        public readonly SignalViewState View;

        public delegate void SignalChanged();
        public SignalChanged ViewChanged;

        public SignalController(int size)
        {
            Internal = new SignalManager(size);
            View = new SignalViewState(Internal);
        }

        public SignalController() : this(DEFAULT_SIZE)
        { }

        public void AddSignal(SinSignal sinSignal)
        {
            Internal.AddSignal(sinSignal);
            View.AddSignal(sinSignal, SignalViewType.Divided);
            ViewChanged();
        }

        public void SwitchView(SignalViewType type)
        {
            View.CurrentType = type;
            ViewChanged();
        }
    }
}

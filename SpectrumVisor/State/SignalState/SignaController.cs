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

        public SignalController(ApplicationState state, int size)
        {
            Internal = new SignalManager(size);
            View = new SignalViewState(state);
        }

        public SignalController(ApplicationState state) : this(state, DEFAULT_SIZE)
        { }
    }
}

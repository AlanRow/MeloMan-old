using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //внутреннее состояние приложения
    //WORK
    //COMPLETE
    class ApplicationState
    {
        public SignalController Signal { get; private set; }

        public ApplicationState()
        {
            Signal = new SignalController();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //возвращает спектр
    interface ISpecGenerator
    {
        FreqPoint[][] GetSpectrum();
        void SwitchOptions(StdOptions opts);
    }
}

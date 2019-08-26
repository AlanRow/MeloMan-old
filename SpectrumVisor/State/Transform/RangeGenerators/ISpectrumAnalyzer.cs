using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //вычисляет один диапазон спектра
    interface ISpectrumAnalyzer
    {
        FreqPoint[] GetRange(StdOptions options);
    }
}

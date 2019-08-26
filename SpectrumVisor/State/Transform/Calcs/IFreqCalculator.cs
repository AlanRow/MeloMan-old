using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //вычисляет част. характ. сигнала при заданной частоте
    interface IFreqCalculator
    {
        Complex CalcFreq(ISignal signal, double freq);
    }
}

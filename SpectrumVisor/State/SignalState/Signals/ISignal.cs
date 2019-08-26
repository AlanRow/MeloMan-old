using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //объект позволяющий получить изменчивое перечисление значений сигнала
    interface ISignal
    {
        IEnumerable<double> GetValues();
        int GetLength();
    }
}

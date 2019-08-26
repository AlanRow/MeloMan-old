using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    interface ITransformer
    {
        //возвращает двумерный спектр
        FreqPoint[][] GetSpectrum();

        //возвращает массив заданных частот
        IEnumerable<double> GetFreqs();

        //изменяет параметры преобразования
        void SwitchOptions(StdOptions newOptions);
    }
}

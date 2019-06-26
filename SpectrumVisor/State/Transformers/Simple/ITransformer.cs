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
        //метод возвращает частотный массив из stepsCount эл-ов signal для частот начиная с w при шаге wStep 
        //Complex[] Transform(double[] signal);
        //метод возвращает спектр для заданного сигнала.
        FreqPoint[][] GetSpectrum();
        void CalcSpectrum();

        //возвращает массив заданных частот
        IEnumerable<double> GetFreqs();

        //изменяет параметры преобразования
        void SwitchOptions(StdOptions newOptions);
    }
}

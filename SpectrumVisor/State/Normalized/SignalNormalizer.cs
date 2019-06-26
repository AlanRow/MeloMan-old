using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class SignalNormalizer
    {
        virtual public double[] Norm(double[] signal)
        {
            var norm = new double[signal.Length];

            var av = signal.Average();

            var avDev = 0d;//среднее отклонение от среднего
            for (var i = 0; i < signal.Length; i++)
            {
                var dev = Math.Abs(av - signal[i]);
                avDev *= i / (i + 1);
                avDev += dev / (i + 1);
            }

            var rising = -av + avDev;//смещение сигнала по высоте

            var avRis = 0d;//сумма со смещением

            for (var i = 0; i < signal.Length; i++)
            {
                avRis *= i / (i + 1);
                avRis += (signal[i] + rising) / (i + 1);
            }

            var normFactor = -1 / (avRis * signal.Length * signal.Length);//множитель нормирования
            //теперь нормализуем сигнал
            for (var i = 0; i < signal.Length; i++)
            {
                norm[i] = (signal[i] + avRis) * normFactor;
            }

            return norm;
        }
    }
}

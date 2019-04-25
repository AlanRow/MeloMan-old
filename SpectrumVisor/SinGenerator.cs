using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //класс, отвечающий за генерацию отдельного периодического сигнала
    class SinGenerator
    {
        //начало сигнала (пока не влияет)
        public double Start { get; private set; }
        //продолжительность
        public double Duration { get; private set; }
        //смещение аргумента
        public double PhaseOffset { get; private set; }
        //множитель частоты
        public double Freq { get; private set; }
        //множитель функции
        public double Mult { get; private set; }
        //прибавочная константа
        public double Const { get; private set; }
        //фактор затухания
        public double Fading { get; private set; }

        private double[] sin;

        public SinGenerator(double start, double dur, double offset, double freq, double mult, double constant, double fading)
        {
            Start = start;
            Duration = dur;
            PhaseOffset = offset;
            Freq = freq;
            Const = constant;
            Fading = fading;
            sin = null;
        }

        public SinGenerator(double dur, double freq, double mult) : this(0, dur, 0, freq, mult, 0, 0)
        {
        }

        public double[] GenerateSin()
        {
            if (sin == null)
            {
                sin = new double[(int)(Math.Floor(Duration) - Math.Ceiling(Start))];

                for (var i = 0; i < (int)Math.Floor(Duration); i++)
                {
                    sin[i] = Math.Sin((i + PhaseOffset) * Freq) * Mult * (1 - i * Fading) + Const;

                    Console.WriteLine(sin[i]);
                }
            }

            return sin;
        }

    }
}

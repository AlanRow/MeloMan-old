using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //агрегирует информацию о сигнале
    class SignalOptions
    {
        //начало сигнала (пока не влияет)
        private int start;
        public int Start
        {
            get { return start; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Начало должно быть неотрицательным целым числом");
                start = value;
            }

        }
        //продолжительность
        private int duration;
        public int Duration
        {
            get { return duration; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Продолжительность сигнала должна быть положительным целым числом");
                duration = value;
            }
        }
        //смещение аргумента (множитель PI)
        private double offset;
        public double PhaseOffset
        {
            get { return offset; }
            set
            {
                if (double.IsNaN(value))
                    throw new ArgumentException("Смещение должно быть вещественным параметром");
                offset = value % 1;
            }
        }
        //множитель частоты
        public double Freq { get; set; }
        //множитель функции
        public double Mult { get; set; }
        //прибавочная константа
        public double Const { get; set; }
        //фактор затухания, не применяется
        //public double Fading { get; set; }

        public SignalOptions(int startIndexx, int durationLength, double frequencyMult, double mult, double constant)
        {
            Start = startIndexx;
            Duration = durationLength;
            Freq = frequencyMult;
            Mult = mult;
            Const = constant;
        }

        public SignalOptions(int startIndex, int durationLength) : this(startIndex, durationLength,
                                                                         durationLength / 8, 1, 0)
        { }

        public string GetTextFormula()
        {
            if (Mult == 0)
                return "0";

            if (Freq == 0)
                return (Math.Sin(PhaseOffset) + Const).ToString();

            var argStr = (PhaseOffset > 0) ? "(x + " + PhaseOffset + ")" :
                (PhaseOffset < 0) ? "(x - " + Math.Abs(PhaseOffset) + ")" : "x";

            var freqStr = (Freq != 1) ? Freq.ToString() + "PI" : "PI";

            var multStr = (Mult != 1) ? Mult + " * " : "";

            var constStr = (Const > 0) ? " + " + Const :
                (Const < 0) ? " - " + Math.Abs(Const) : "";

            return multStr + "sin(" + freqStr + argStr + ")" + constStr;
        }
    }
}

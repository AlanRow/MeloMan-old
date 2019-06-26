using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //служит для объединения свойств преобразования
    class StdOptions
    {
        private double start;
        private double step;
        private int count;

        public double StartFreq
        {
            get { return start; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Start frequency must be more than 0!");
                start = value;
            }
        }

        public double StepFreq
        {
            get { return step; }
            set
            {
                if (value * count + start <= 0)
                    throw new ArgumentException("Frequency must be more than 0 everytime!");
                step = value;
            }
        }

        public int CountFreq
        {
            get { return count; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Steps count must be more than 0!");
                count = value;
            }
        }

        public StdOptions()
        {
            start = 0.1;
            step = 0.1;
            count = 30;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class FreqPoint
    {
        public readonly Complex Coords;
        public readonly double Freq;

        public FreqPoint(Complex coords, double w)
        {
            Coords = coords;
            Freq = w;
        }
    }
}

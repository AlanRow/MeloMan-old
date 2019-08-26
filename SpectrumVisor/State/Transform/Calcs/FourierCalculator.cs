using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    class FourierCalculator : IFreqCalculator
    {
        public Complex CalcFreq(ISignal signal, double w)
        {
            var mc = Complex.Zero;
            var i = 0;
            foreach (var val in signal.GetValues())
                mc += val * Complex.FromPolarCoordinates(1, -w * i / signal.GetLength() * 2 * Math.PI);

            return mc;
        }
    }
}

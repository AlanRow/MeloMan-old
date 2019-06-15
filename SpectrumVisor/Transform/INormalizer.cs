using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor.Transform
{
    interface INormalizer
    {
        double Norm(double[] signal);
    }
}

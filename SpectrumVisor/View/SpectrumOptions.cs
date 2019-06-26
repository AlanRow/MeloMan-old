using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    interface SpectrumOptions
    {
        void UpdateSpectrum(FreqPoint[][] newSpec);
    }
}

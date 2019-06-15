using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    interface IWindowedTransformer : ITransformer
    {
        double GetWinSize();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    abstract class SpectrumView : Panel
    {
        abstract public void Update(FreqPoint[][] newSpec);
    }
}

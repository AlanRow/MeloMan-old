using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    class OptionsGenerator
    {
        private TransformManager transform;

        public OptionsGenerator(TransformManager manager)
        {
            transform = manager;
        }

        public OptionsPanel GetPanel(TransformType type)
        {
            switch (type)
            {
                case TransformType.Fourier:
                        return new StandartOptionsPanel(transform);
                case TransformType.Windowed:
                    return new WindowedOptions(transform);
                case TransformType.Gabor:
                    throw new NotImplementedException("Gabor Filter isn't realized now!");
            }

            return null;
        }
    }
}

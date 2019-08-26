using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //позволяет взаимодействовать с преобразованием (изменять тип представления, параметры сигнала)
    class TransformController
    {
        public readonly TransformManager Transform;
        public readonly TransformViewState View;
    }
}

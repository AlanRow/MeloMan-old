using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    //тип представления спектра
    enum ViewVersion
    {
        Linear,
        Round,
        Color
    }

    //класс инкапсулирует в себя логику размещения и смены визуального представления спектра
    class SpectrumViewManager : Panel
    {
        private SpectrumView view;

        public SpectrumViewManager(FreqPoint[][] spectrum)
        {
            var opts = new RoundOptions(spectrum);
            view = new RoundSpectrum(opts);
            view.Dock = DockStyle.Fill;
            view.Invalidated += (sender, ev) => { Invalidate(); };
            Controls.Add(view);
        }

        public void Update(FreqPoint[][] newSpectrum)
        {
            view.Update(newSpectrum);
        }
    }
}

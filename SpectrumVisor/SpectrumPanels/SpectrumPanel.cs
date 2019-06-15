using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    class SpectrumPanel : Panel
    {
        private SignalManager signal;
        private TransformManager spectrum;
        private SpectrumViewManager view;

        private OptionsPanel options;

        public SpectrumPanel(SignalManager manager, TransformManager transformer )
        {
            signal = manager;
            spectrum = transformer;

            view = new SpectrumViewManager(spectrum.GetSpectrum());
            options = new OptionsPanel(spectrum);
            
            Controls.Add(options);
            Controls.Add(view);
            Update();

            spectrum.Retransformed += () =>
            {
                Update();
            };

            SizeChanged += (sender, ev) =>
            {
                var chartSize = Math.Min(Width, Height * 70 / 100);
                view.Size = new Size(chartSize, chartSize);
                options.SetBounds(0, chartSize + 25, Width, Math.Max(250, Height / 4));

                view.Invalidate();
            };
        }

        private void Update()
        {
            view.Update(spectrum.GetSpectrum());
            Invalidate();
        }
    }
}

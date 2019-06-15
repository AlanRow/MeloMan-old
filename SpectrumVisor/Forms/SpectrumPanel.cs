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
        private enum ViewVersion
        {
            Linear,
            Round,
            Color
        }

        private SignalManager signal;
        private TransformManager spectrum;

        private ViewVersion viewVariant;

        private Dictionary<ViewVersion, Panel> views;
        private Panel currentView;

        private OptionsPanel options;

        public SpectrumPanel(SignalManager manager, TransformManager transformer )
        {
            signal = manager;
            spectrum = transformer;

            viewVariant = ViewVersion.Round;

            currentView = new Panel();
            options = new OptionsPanel(spectrum);

            Controls.Add(currentView);
            Controls.Add(options);
            

            views = new Dictionary<ViewVersion, Panel>();
            Update();

            spectrum.Retransformed += () =>
            {
                Update();
            };

            SizeChanged += (sender, ev) =>
            {
                var chartSize = Math.Min(Width, Height * 70 / 100);
                currentView.Size = new Size(chartSize, chartSize);
                options.SetBounds(0, chartSize + 25, Width, Math.Max(250, Height / 4));

                currentView.Invalidate();
            };
        }

        private void InitViews()
        {
            var spec = spectrum.GetSpectrum();
            var freqs = spectrum.GetFreqsValues();
            var freqPoints = new FreqPoint[spec.Length][];

            //по идее это и должен выдавать GetSpectrum
            for (var i = 0; i < spec.Length; i++)
            {
                freqPoints[i] = new FreqPoint[spec[0].Length];

                for (var j = 0; j < spec[0].Length; j++)
                    freqPoints[i][j] = new FreqPoint(spec[i][j], freqs[j]);
            }


            //разные представления спектра
            //views[ViewVersion.Linear] = new LinearSpectrum(spec);
            //views[ViewVersion.Color] = new ColorSpectrum(spec);
            var oneSpec = freqPoints.Select((arr) => arr[0]).ToArray();
            views[ViewVersion.Round] = new RoundSpectrum(new RoundOptions(freqPoints));
        }

        private void Update()
        {
            InitViews();
            Controls.Remove(currentView);
            currentView = views[viewVariant];
            //currentView.Dock = DockStyle.Fill;
            Controls.Add(currentView);
            OnSizeChanged(EventArgs.Empty);
            Invalidate();
        }

        //public void SwitchView()
        //{
        //    Controls.Remove(currentView);
        //    currentView = views[viewVariant];
        //    currentView.Dock = DockStyle.Fill;
        //    Controls.Add(currentView);
        //    Invalidate();
        //}
    }
}

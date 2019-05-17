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

        private Dictionary<ViewVersion, PictureBox> views;
        private PictureBox currentView;

        private OptionsPanel options;

        public SpectrumPanel(SignalManager manager, TransformManager transformer )
        {
            signal = manager;
            spectrum = transformer;

            viewVariant = ViewVersion.Round;

            currentView = new PictureBox();
            options = new OptionsPanel(spectrum);

            Controls.Add(currentView);
            Controls.Add(options);
            

            views = new Dictionary<ViewVersion, PictureBox>();
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
            //разные представления спектра
            //views[ViewVersion.Linear] = new LinearSpectrum(spec);
            //views[ViewVersion.Color] = new ColorSpectrum(spec);
            views[ViewVersion.Round] = new RoundSpectrum(spec);
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

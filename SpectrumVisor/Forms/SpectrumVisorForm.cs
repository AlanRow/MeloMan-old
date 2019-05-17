using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SpectrumVisor
{
    class SpectrumVisorForm : Form
    {
        private SignalPanel signalPanel;
        private SpectrumPanel spectrumPanel;

        public SpectrumVisorForm() : base()
        {
            Width = 1000;
            Height = 800;

            var signal = new SignalManager(10, 1024);
            //signal.AddSignalBySize(0, 1024, 0, 1.0/ 256, 10, 0, 0);
            //signal.AddSignalBySize(0, 1024, 0, 1.0 / 64, 5, 0, 0);
            //signal.AddSignalBySize(0, 1024, 0, 1.0 / 32, 1, 5, 0);

            var transform = new TransformManager(new FourierTransformer(), signal);
            
            signalPanel = new SignalPanel(signal);
            signalPanel.MaximumSize = new Size(400, 600);

            spectrumPanel = new SpectrumPanel(signal, transform);

            Controls.Add(signalPanel);
            Controls.Add(spectrumPanel);


            Load += (sender, ev) => OnSizeChanged(EventArgs.Empty);
            SizeChanged += (sender, ev) =>
            {
                signalPanel.Size = new Size(Width * 40 / 100, Height - 100);
                spectrumPanel.SetBounds(Width * 45 / 100, 0, Width / 2, Height - 100);
            };
        }
    }
}

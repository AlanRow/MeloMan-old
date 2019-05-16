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
            signal.AddSignalBySize(0, 1024, 0, 1.0/ 256, 10, 0, 0);
            signal.AddSignalBySize(0, 1024, 0, 1.0 / 64, 5, 0, 0);
            signal.AddSignalBySize(0, 1024, 0, 1.0 / 32, 1, 5, 0);

            var transform = new TransformManager(new FourierTransformer(), signal);

            var table = new FlowLayoutPanel();
            table.FlowDirection = FlowDirection.LeftToRight;

            signalPanel = new SignalPanel(signal);

            table.Width = 600;
            table.Height = 800;

            signalPanel.Width = 600;
            signalPanel.Height = 800;

            spectrumPanel = new SpectrumPanel(signal, transform);
            //spectrumPanel.Location = new Point(450, 0);

            //table.Controls.Add(signalPanel);


            //table.Controls.Add(spectrumPanel);
            //Controls.Add(table);
            Controls.Add(signalPanel);

            //Controls.Add(new SignalsList(signal));
        }
    }
}

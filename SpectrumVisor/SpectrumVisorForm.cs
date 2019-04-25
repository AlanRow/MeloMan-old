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
        public SpectrumVisorForm()
        {
            Width = 1000;
            Height = 800;

            var manager = new SignalManager(10, 1024);
            manager.AddSignalBySize(0, 1024, 0, 1, 1, 0, 0);

            signalPanel = new SignalPanel(manager);
            signalPanel.Width = 400;
            signalPanel.Height = 600;

            Controls.Add(signalPanel);
        }
    }
}

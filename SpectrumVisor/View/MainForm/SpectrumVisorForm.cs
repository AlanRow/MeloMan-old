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
        private ApplicationState state;
        private SignalPanel signalPanel;
        //private SpectrumPanel spectrumPanel;

        public SpectrumVisorForm() : base()
        {
            //инициализация состояния
            state = new ApplicationState();

            //корректировка размеров
            Width = ClientRectangle.Width;
            Height = ClientRectangle.Height;
            
            //создание панелей
            signalPanel = new SignalPanel(state);
            //spectrumPanel = new SpectrumPanel(state);

            //добавление панелей
            Controls.Add(signalPanel);
            //Controls.Add(spectrumPanel);


            Load += (sender, ev) => OnSizeChanged(EventArgs.Empty);
            SizeChanged += (sender, ev) =>
            {
                signalPanel.Size = new Size(Width * 40 / 100, Height - 100);
                //spectrumPanel.SetBounds(Width * 45 / 100, 0, Width / 2, Height - 100);
            };
        }
    }
}

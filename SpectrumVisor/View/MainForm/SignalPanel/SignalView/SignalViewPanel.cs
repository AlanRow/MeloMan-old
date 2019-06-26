using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    //размещает на форме график и кнопку смены представления
    class SignalViewPanel : Panel
    {
        public SignalViewPanel(ApplicationState state)
        {
            //кнопка изменения типа представления
            //var viewButton = new SignalViewChangeButton(state);
            //панель представления сигнала
            var viewer = new SignalChart(state);

            //viewButton.Dock = DockStyle.Top;
            viewer.Dock = DockStyle.Fill;

            //Controls.Add(viewButton);
            Controls.Add(viewer);
        }
    }
}

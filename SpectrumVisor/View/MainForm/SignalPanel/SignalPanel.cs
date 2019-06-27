using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace SpectrumVisor
{

    //отвечает за размещение информации о сигнале
    //содержит:
    /*
     * 1. панель изображения сигнала
     * 2. панель со списком сигналов
     */
    class SignalPanel : Panel
    {
        static private Size minViewSize = new Size(200, 150);

        private SignalViewPanel view;
        private SignalsList list;

        public SignalPanel(ApplicationState state) : base()
        {
            view = new SignalViewPanel(state);
            list = new SignalsList(state);

            SizeChanged += (sender, ev) =>
            {
                view.SetBounds(0, 0, Height / 2, Height / 2);

                var listPrefs = list.PreferredSize;
                list.SetBounds(0, view.Height,
                                      listPrefs.Width, listPrefs.Height);
            };

            Controls.Add(view);
            Controls.Add(list);
            Invalidate();
        }
    }
}

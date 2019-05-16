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

        private TableLayoutPanel table;

        private ViewVersion viewVariant;

        private Dictionary<ViewVersion, PictureBox> views;
        private PictureBox currentView;

        private OptionsPanel options;

        public SpectrumPanel(SignalManager manager, TransformManager transformer )
        {
            signal = manager;
            spectrum = transformer;


            Width = 400;
            Height = 600;

            var log = new Logger("spec_pan_size.txt");
            log.WriteLog("Width: " + Width);
            log.WriteLog("Height: " + Height);
            log.Flush();

            //Dock = DockStyle.Fill;

            viewVariant = ViewVersion.Round;

            table = new TableLayoutPanel();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            currentView = new PictureBox();
            options = new OptionsPanel(spectrum);
            options.Width = 400;
            options.Height = 400;

            table.Controls.Add(currentView, 0, 0);
            table.Controls.Add(options, 0, 1);

            Controls.Add(table);

            views = new Dictionary<ViewVersion, PictureBox>();

            spectrum.Retransformed += () =>
            {
                update();
            };

            update();
        }

        private void update()
        {
            var spec = spectrum.GetSpectrum();

            //разные представления спектра
            //views[ViewVersion.Linear] = new LinearSpectrum(spec);
            views[ViewVersion.Round] = new RoundSpectrum(spec);
            //views[ViewVersion.Color] = new ColorSpectrum(spec);

            SwitchView();
        }

        public void SwitchView()
        {
            table.Controls.Remove(currentView);
            currentView = views[viewVariant];
            currentView.Dock = DockStyle.Fill;
            table.Controls.Add(currentView, 0, 0);
            Invalidate();
        }
    }
}

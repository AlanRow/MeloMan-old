using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    class WindowTracker : Panel
    {
        public WindowTracker(RoundOptions options)
        {

            var trackLabel = new Label
            {
                Font = options.TextFont,
                Text = String.Format("Центр окна: {0}\n", 0),
                AutoSize = true,
                Location = new Point(ClientRectangle.Width / 2, 0)
            };

            //создание бегунка для смещения центра окна
            var winTrack = new TrackBar
            {
                Orientation = Orientation.Horizontal,
                TickFrequency = options.WinStep,
                Maximum = options.SpecSize,
                Location = new Point(0, trackLabel.Height),
                TickStyle = TickStyle.BottomRight
            };

            winTrack.Scroll += (sender, ev) =>
            {
                options.ViewSpec(winTrack.Value);
                trackLabel.Text = String.Format("Центр окна: {0}\n", winTrack.Value);
                Invalidate();
            };

            Controls.Add(winTrack);
            Controls.Add(trackLabel);

            Resize += (sender, ev) =>
            {
                trackLabel.Location = new Point((ClientRectangle.Width - trackLabel.Width) / 2, 0);
                winTrack.SetBounds(0, trackLabel.Height, ClientRectangle.Width, 50);
            };

        }
    }
}

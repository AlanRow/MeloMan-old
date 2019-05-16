using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    //InputFieldGenerator
    static class IFG
    {
        public static Panel InitDoubleField(string label, Action<double> setter)
        {
            return InitInputField(label, (value) =>
            {
                var dValue = 0d;
                if (double.TryParse(value, out dValue))
                    setter(dValue);
            });
        }

        public static Panel InitIntField(string label, Action<double> setter)
        {
            return InitInputField(label, (value) =>
            {
                var iValue = 0;
                if (int.TryParse(value, out iValue))
                    setter(iValue);
            });
        }


        public static Panel InitInputField(string label, Action<string> setter)
        {
            var pan = new Panel();
            pan.Dock = DockStyle.Fill;

            var title = new Label
            {
                Location = new Point(0, 0),
                Height = 30,
                Font = new System.Drawing.Font("Arial", 10),
                Text = label
            };

            var field = new TextBox
            {
                Location = new Point(0, 50),
            };

            field.TextChanged += (sender, ev) =>
            {
                setter(field.Text);
            };

            pan.Controls.Add(title);
            pan.Controls.Add(field);

            return pan;
        }
    }
}

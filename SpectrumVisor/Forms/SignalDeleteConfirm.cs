﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    class SignalDeleteConfirm : Form
    {
        public SignalDeleteConfirm(SignalManager manager, int signalNumber)
        {
            MaximumSize = new Size(300, 200);
            MinimumSize = new Size(300, 200);

            var message = new Label
            {
                Text = "Вы уверены что хотите удалить сигнал " + manager.Signals[signalNumber].GetTextFormula() + " из списка сигналов.",
                Location = new Point(50, 100)
            };

            var okButton = new Button
            {
                Location = new Point(100, 150),
                Text = "Да"
            };
            okButton.Click += (sender, ev) => { manager.DeleteSignal(signalNumber); };

            var cancelButton = new Button()
            {
                Location = new Point(200, 150),
                Text = "Нет"
            };
            cancelButton.Click += (sender, ev) => { Close(); };

            Controls.Add(message);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
        }
    }
}
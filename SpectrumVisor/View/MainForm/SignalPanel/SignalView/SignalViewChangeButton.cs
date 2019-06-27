using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    class SignalViewChangeBox : ComboBox
    {
        public SignalViewChangeBox(ApplicationState state)
        {
            //прописываем вручную, а надо бы автоматически
            Items.Add(new BoxItem(SignalViewType.Divided, "Отдельные"));
            Items.Add(new BoxItem(SignalViewType.Summarized, "Суммированный"));

            SelectedIndexChanged += (sender, ev) => {
                state.Signal.SwitchView(((BoxItem)SelectedItem).Type);
            };
        }

        //добавляет enum'у строковое представление
        class BoxItem
        {
            public readonly SignalViewType Type;
            public string Name { get; private set; }

            public BoxItem(SignalViewType type, string name)
            {
                Type = type;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumVisor
{
    enum TransformType
    {
        Fourier,
        Windowed,
        Gabor
    }

    //агрегирует и переключает панели настроек преобразования
    class OptionsManager : Panel
    {
        //преобразователь
        private TransformManager transformer;
        //генератор панелей опций
        private OptionsGenerator generator;
        //переключатель типов преобразования
        private ComboBox switcher;
        //словарь с условными названиями типов преобразований
        private Dictionary<TransformType, string> typeNames;
        //обратный словарь
        private Dictionary<string, TransformType> nameTypes;
        //панель опций
        private OptionsPanel optPanel;

        public OptionsManager(TransformManager transform)
        {
            transformer = transform;

            typeNames = new Dictionary<TransformType, string>
            {
                [TransformType.Fourier] = "Преобразование Фурье",
                [TransformType.Windowed] = "Оконное преобразование Фурье"
            };

            nameTypes = new Dictionary<string, TransformType>();
            foreach (var pair in typeNames)
            {
                nameTypes[pair.Value] = pair.Key;
            }

            generator = new OptionsGenerator(transformer);

            switcher = new ComboBox();
            switcher.Items.Add(typeNames[TransformType.Fourier]);
            switcher.Items.Add(typeNames[TransformType.Windowed]);
            switcher.SelectedText = typeNames[TransformType.Fourier];
            switcher.SelectedIndexChanged += (sender, ev) =>
            {
                Switch(nameTypes[switcher.SelectedItem.ToString()]);
            };

            optPanel = generator.GetPanel(TransformType.Fourier);

            switcher.Dock = DockStyle.Top;
            optPanel.Dock = DockStyle.Fill;

            Controls.Add(switcher);
            Controls.Add(optPanel);
        }

        public void Switch(TransformType newType)
        {
            switcher.Text = typeNames[newType];
            transformer.SwitchTransform(newType);

            Controls.Remove(optPanel);
            Controls.Add(generator.GetPanel(newType));
        }
    }
}

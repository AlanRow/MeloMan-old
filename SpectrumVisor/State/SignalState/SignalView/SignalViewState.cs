using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //предоставляет информацию о текущем представлении сигнала/сигналов
    //WORK
    //INCOMPLETE
    class SignalViewState
    {
        public static SignalViewType DEFAULT_VIEW = SignalViewType.Summarized;

        public SignalViewType CurrentType { get; set; }
        private Dictionary<SignalViewType, ISignalsVisualizer> views;

        public SignalViewState(SignalManager manager, SignalViewType current)
        {
            views = new Dictionary<SignalViewType, ISignalsVisualizer>();
            AddSignal( new AloneVisualizer(manager.Sum), SignalViewType.Summarized);
            AddSignal(new ManagerVisualizer(manager), SignalViewType.Divided);

            var divided = new List<SignalViewOptions>();

            CurrentType = current;
        }

        public SignalViewState(SignalManager manager) : this(manager, DEFAULT_VIEW) { }
        
        public void AddSignal(ISignalsVisualizer signals, SignalViewType type)
        {
            views[type] = signals;
        }

        public ISignalsVisualizer GetCurrentViews()
        {
            return views[CurrentType];
        }

        //текущее название сигнала
        public string GetCurrentName()
        {
            return "Default";
        }
    }
}

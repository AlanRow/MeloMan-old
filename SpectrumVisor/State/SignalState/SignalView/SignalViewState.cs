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
        public SignalViewType CurrentType { get; set; }

        private Dictionary<SignalViewType, ISignalsVisualizer> views;

        public SignalViewState(ApplicationState state, SignalViewType current)
        {
            views = new Dictionary<SignalViewType, ISignalsVisualizer>();
            AddSignal( new AloneVisualizer(state.InternalState.Signal.Sum), SignalViewType.Summarized);
            AddSignal(new ManagerVisualizer(state.InternalState.Signal), SignalViewType.Divided);

            var divided = new List<SignalViewOptions>();
            


            CurrentType = current;
        }

        public SignalViewState(ApplicationState state) : this(state, SignalViewType.Summarized) { }

        //RECONSTRUCT
        public void AddSignal(ISignalsVisualizer signals, SignalViewType type)
        {
            //views[type] = signals;
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

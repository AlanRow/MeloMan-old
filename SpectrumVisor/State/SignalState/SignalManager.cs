using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //агрегирует информацию об исходных сигналах
    //сообщает о создании и удалении сигнала
    class SignalManager
    {
        //исходные сигналы
        public List<SinSignal> Signals { get; private set; }
        public SumSignal Sum { get; private set;}

        //общая длительность по времени (не используется)
        //public double TimeDuration { get; private set; }
        public int Size { get; private set; }


        public delegate void SignalsChanged(SinSignal gener);
        public event SignalsChanged AddedSignal;
        public event SignalsChanged DeletedSignal;

        public SignalManager(int size)
        {
            Signals = new List<SinSignal>();
            Sum = new SumSignal(this);
            Size = size;
        }

        public void AddSignal(SinSignal signal)
        {
            Signals.Add(signal);

            if (AddedSignal != null)
                AddedSignal(signal);
        }

        public void DeleteSignal(SinSignal signal)
        {
            var i = Signals.IndexOf(signal);

            if (i >= 0)
                Signals.RemoveAt(i);


            if (AddedSignal != null)
                DeletedSignal(signal);
        }
    }
}

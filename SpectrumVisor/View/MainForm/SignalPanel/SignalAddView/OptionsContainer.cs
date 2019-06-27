using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //позволяет осуществить автоматическую генерацию сигнала из текстовых полей
    class OptionsParser
    {
        public List<string> ErrorMessages;

        public SubscribedField Start;
        public SubscribedField Duration;
        public SubscribedField Frequency;
        public SubscribedField Mult;
        public SubscribedField Const;

        public OptionsParser()
        {
            ErrorMessages = new List<string>();
        }

        public SignalOptions GetOptions()
        {
            ErrorMessages = new List<string>();

            var start = initIntPar(Start);
            var dur = initIntPar(Duration);
            var freq = initDoublePar(Frequency);
            var mult = initDoublePar(Mult);
            var c = initDoublePar(Const);

            if (ErrorMessages.Count > 0)
                return null;

            return new SignalOptions(start.Value, dur.Value, freq, mult, c);
        }

        private int? initIntPar(SubscribedField form)
        {
            try
            {
                return int.Parse(form.Field.Text);
            } catch (FormatException ex)
            {
                ErrorMessages.Add(form.ErrorMessage);
                return null;
            }
        }

        private double initDoublePar(SubscribedField form)
        {
            try
            {
                return double.Parse(form.Field.Text);
            }
            catch (FormatException ex)
            {
                ErrorMessages.Add(form.ErrorMessage);
                return double.NaN;
            }
        }
    }
}

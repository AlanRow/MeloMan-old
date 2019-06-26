using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumVisor
{
    //оконное преобразование Фурье
    //class WindowedFourier : FourierTransformer
    //{
        //public readonly int WinSize;
        //protected WindowedNormalizer normalizer;
        //private int start;

        //public WindowedFourier(SignalManager signal, int winSize) : base(signal)
        //{
        //    WinSize = winSize;
        //    start = -WinSize / 2;
        //    normalizer = new WindowedNormalizer(WinSize);
        //    CalcSpectrum(signal.GetSum());
        //}

        //public WindowedFourier() : this(256) { }

        //protected override Complex findMC(double[] signal, double w)
        //{
        //    var mc = Complex.Zero;

        //    for (var i = start; i < start + WinSize; i++)
        //    {
        //        var ind = (mod(i, signal.Length));
        //        mc += signal[ind] * Complex.FromPolarCoordinates(1, -w * i / WinSize * 2 * Math.PI);
        //    }

        //    return mc;
        //}

        //protected override async void CalcSpectrum(double[] signal)
        //{
        //    var spec = new FreqPoint[spectrum.Length][];
        //    start = 0;
        //    await Task.Run(() =>
        //    {
        //        for (var i = 0; i < spectrum.Length; i++)
        //        {
        //            spectrum[i] = Transform(GetWindow(signal));
        //        }

        //    });
        //}

        //private static int mod(int number, int basis)
        //{
        //    var m = number % basis;

        //    return (m >= 0) ? m : basis + m;
        //}

        //private double[] GetWindow(double[] signal)
        //{
        //    var win = new double[signal.Length];

        //    for (var i = 0; i < win.Length; i++)
        //        win[i] = (i >= start && i < start + WinSize) ? signal[i] : 0;

        //    return win;
        //}

        //public double GetWinSize()
        //{
        //    return WinSize;
        //}
    //}
}

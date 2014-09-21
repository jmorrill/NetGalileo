using System;
using System.Threading;
using Embprpusr.Managed;

namespace Embprpusr.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            const int pin13Map = 39;

            int hr = 0;

            EmbprpusrInterop.GpioSetDir(4, 1);
            EmbprpusrInterop.GpioWrite(4, 1);

            hr = EmbprpusrInterop.GpioSetDir(pin13Map, 1);

            Console.WriteLine("GpioSetDir returned: {0}", hr);
            while (true)
            {
                hr = EmbprpusrInterop.GpioWrite(pin13Map, 0);
                Console.WriteLine("GpioWrite returned: {0}", hr);

                Thread.Sleep(500);
                
                hr = EmbprpusrInterop.GpioWrite(pin13Map, 1);
                Console.WriteLine("GpioWrite returned: {0}", hr);

                Thread.Sleep(500);
            }
        }
    }
}

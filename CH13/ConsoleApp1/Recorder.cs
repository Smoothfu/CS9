using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static System.Console;
using static System.Diagnostics.Process;


namespace ConsoleApp1
{
    public static class Recorder
    {
        static Stopwatch sw = new Stopwatch();
        static long bytesPhysicalBefore = 0;
        static long bytesVirtualBefore = 0;

        public static void Start()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            bytesPhysicalBefore = GetCurrentProcess().WorkingSet64;
            bytesVirtualBefore = GetCurrentProcess().VirtualMemorySize64;
            sw.Restart();
        }

        public static void Stop()
        {
            sw.Stop();
            long bytesPhysicalAfter = GetCurrentProcess().WorkingSet64;
            long bytesVirtualAfter = GetCurrentProcess().VirtualMemorySize64;
            WriteLine("{0:N0} physical bytes used.", bytesPhysicalAfter - bytesPhysicalBefore);
            WriteLine("{0:N0} virtual bytes used.", bytesVirtualAfter - bytesVirtualBefore);
            WriteLine("{0:N0} total milliseconds ellapsed.", sw.ElapsedMilliseconds);

        }
    }
}

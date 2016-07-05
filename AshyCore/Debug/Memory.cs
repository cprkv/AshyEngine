using System;
using System.Diagnostics;
using System.Runtime;

namespace AshyCore.Debug
{
    public static class Memory
    {
        private static bool _noGC = false;

        public static void Collect(bool showLog = true)
        {
            GC.Collect                      ();
            GC.WaitForPendingFinalizers     ();
            var currentProc                 = Process.GetCurrentProcess();
            if (showLog)
            {
                CoreAPI.I.Core.Log.Info     ( $"[Core] Ñollecting memory. End status: {currentProc.PrivateMemorySize64/1024/1024} Mb" );
            }
        }

        public static void CompactHeap()
        {
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect                      ( GC.MaxGeneration, GCCollectionMode.Forced, true, true );
            GC.WaitForPendingFinalizers     ();
        }

        public static bool NoGC
        {
            get
            {
                return                      ( _noGC );
            }
            set
            {   
                if (value == _noGC)         return;

                if (value && ! _noGC)
                {
                    CompactHeap             ();
                    GCSettings.LatencyMode  = GCLatencyMode.LowLatency;
                    _noGC                   = true;
                }
                else
                {
                    GCSettings.LatencyMode  = GCLatencyMode.Interactive;
                    _noGC                   = false;
                }
            }
        }
    }
}
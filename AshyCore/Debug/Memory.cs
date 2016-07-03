using System;
using System.Diagnostics;

namespace AshyCore.Debug
{
    public static class Memory
    {
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
    }
}
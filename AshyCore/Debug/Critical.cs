using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshyCore.Debug
{
    public static class Critical
    {
        public static T NoThrow<T>(this Func<T> func) where T : class
        {
            try
            {
                return func();
            }
            catch
            {
                return null;
            }
        }

        public static void NoThrow(this Action func)
        {
            try
            {
                func();
            }
            catch
            {
            }
        }

        public static void CollectMemory(bool showLog = true)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var currentProc = Process.GetCurrentProcess();
            if (showLog)
            {
                CoreAPI.I.Core.Log.Info($"[Core] Сollecting memory. End status: {currentProc.PrivateMemorySize64/1024/1024} Mb");
            }
        }
    }
}

// 
// Created : 16.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace AshyCore.Debug
{
    /// <summary>
    /// Dump writer
    /// </summary>
    public static class MiniDumpWriter
    {
        [Flags]
        private enum MinidumpType
        {
            MiniDumpNormal                          = 0x00000000,
            MiniDumpWithDataSegs                    = 0x00000001,
            MiniDumpWithFullMemory                  = 0x00000002,
            MiniDumpWithHandleData                  = 0x00000004,
            MiniDumpFilterMemory                    = 0x00000008,
            MiniDumpScanMemory                      = 0x00000010,
            MiniDumpWithUnloadedModules             = 0x00000020,
            MiniDumpWithIndirectlyReferencedMemory  = 0x00000040,
            MiniDumpFilterModulePaths               = 0x00000080,
            MiniDumpWithProcessThreadData           = 0x00000100,
            MiniDumpWithPrivateReadWriteMemory      = 0x00000200,
            MiniDumpWithoutOptionalData             = 0x00000400,
            MiniDumpWithFullMemoryInfo              = 0x00000800,
            MiniDumpWithThreadInfo                  = 0x00001000,
            MiniDumpWithCodeSegs                    = 0x00002000
        }

        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);

        public static bool WriteLog()
        {
            MinidumpType options =
                  MinidumpType.MiniDumpWithDataSegs
                | MinidumpType.MiniDumpWithUnloadedModules
                | MinidumpType.MiniDumpWithProcessThreadData
                | MinidumpType.MiniDumpScanMemory
                | MinidumpType.MiniDumpWithFullMemoryInfo
                //| MinidumpType.MiniDumpWithFullMemory
                ;

            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".dmp");

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
            {
                var currentProcess          = Process.GetCurrentProcess();
                var currentProcessHandle    = currentProcess.Handle;
                var currentProcessId        = (uint)currentProcess.Id;

                return MiniDumpWriteDump(currentProcessHandle, currentProcessId, fs.SafeFileHandle, (uint)options, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }
    }
}
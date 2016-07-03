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
    public static class MiniDump
    {
        /// <summary>
        /// Identifies the type of information that will be written to the minidump file by the <see cref="MiniDumpWriteDump"/> function.
        /// </summary>
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


        #region DLL Import function

        /// <summary>
        ///     Writes user-mode minidump information to the specified file.
        /// </summary>
        /// 
        /// <param name="hProcess">
        ///     A handle to the process for which the information is to be generated.
        /// </param>
        /// <param name="processId">
        ///     The identifier of the process for which the information is to be generated.
        /// </param>
        /// <param name="hFile">
        ///     A handle to the file in which the information is to be written.
        /// </param>
        /// <param name="dumpType">
        ///     The type of information to be generated. <see cref="MinidumpType"/>
        /// </param>
        /// <param name="expParam">
        ///     A pointer to a MINIDUMP_EXCEPTION_INFORMATION structure describing the client exception that caused the minidump to be generated. 
        ///     If the value of this parameter is NULL, no exception information is included in the minidump file.
        /// </param>
        /// <param name="userStreamParam">
        ///     A pointer to a MINIDUMP_USER_STREAM_INFORMATION structure. 
        ///     If the value of this parameter is NULL, no user-defined information is included in the minidump file.
        /// </param>
        /// <param name="callbackParam">
        ///     A pointer to a MINIDUMP_CALLBACK_INFORMATION structure that specifies a callback routine which is to receive extended minidump information. 
        ///     If the value of this parameter is NULL, no callbacks are performed.
        /// </param>
        /// 
        /// <returns>
        ///     <code>true</code> when writing dump is successful.
        /// </returns>
        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);

        #endregion


        /// <summary>
        /// Writes memory dump to the application folder.
        /// Dump name generates automatically in format: "yyyyMMdd-HHmmss.dmp".
        /// </summary>
        /// <returns><code>true</code>, if writing is successful.</returns>
        public static bool Write()
        {
            uint options                        = (uint) ( MinidumpType.MiniDumpWithDataSegs
                                                         | MinidumpType.MiniDumpWithUnloadedModules
                                                         | MinidumpType.MiniDumpWithProcessThreadData
                                                         | MinidumpType.MiniDumpScanMemory
                                                         | MinidumpType.MiniDumpWithFullMemoryInfo
                                                       //| MinidumpType.MiniDumpWithFullMemory          // uncomment this if your wanna see all memory. dump will be BIG!
                                                         );
            string fileName                     = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, 
                                                                DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".dmp" );

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
            {
                Process currentProcess          = Process.GetCurrentProcess();

                bool attemptWriteDump           = MiniDumpWriteDump( hProcess:        currentProcess.Handle, 
                                                                     processId:       (uint) currentProcess.Id, 
                                                                     hFile:           fs.SafeFileHandle, 
                                                                     dumpType:        options, 
                                                                     expParam:        IntPtr.Zero, 
                                                                     userStreamParam: IntPtr.Zero, 
                                                                     callbackParam:   IntPtr.Zero );

                return                          ( attemptWriteDump );
            }
        }
    }
}
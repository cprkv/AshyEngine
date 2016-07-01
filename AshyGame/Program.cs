// 
// Created : 10.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Runtime.InteropServices;
using AshyCore;
using System.Windows.Forms;
using AshyCore.Debug;
using AshyCore.EngineAPI;

namespace AshyGame
{
    internal class EntryPoint
    {
        //private static string[] _args;

        private static void UnhandledExceptionFilter(object sender, UnhandledExceptionEventArgs e)
        {
            EngineFailed                        ( e.ExceptionObject.ToString(), "Unhandled Exception" );
        }

        static void EngineFailed(string allText, string title)
        {
#if ! DEBUG
            MiniDumpWriter.WriteLog             ();
#endif
            MessageBox.Show                     (
                                                    allText,
                                                    title,
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error
                                                );
            Critical.NoThrow(() => 
            {
                GameAPI.I.Core.Log.Error        ( "--- " + title );
                GameAPI.I.Core.Log.Error        ( allText );
                GameAPI.I.Core.Log.End          ();
            });
        }

        private static void Main(string[] args)
        {
           // _args = args;
            Debug.ProtectedCall                 ( MainProtected, args );
        }

        private static void MainProtected(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionFilter;

            if (Application.Preinitialize(args) == EngineStatus.ReadyToLoad)
            {
                if (Application.Initialize() == EngineStatus.ReadyToUse)
                {
                    Application.Execute         ();
                }
                else
                {
                    EngineFailed                ( "Engine Initialization failed!", "AshyEngine Error" );
                }                                 
            }                                     
            else                                  
            {                                     
                EngineFailed                    ( "Engine Preinitialization failed!", "AshyEngine Error" );
            }                                     

            Application.Free                    ();
        }
    }
}

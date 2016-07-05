// 
// Created : 25.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using AshyCore.Debug;
using AshyCore.VFS;
using AshyCore.EngineAPI;
using IniParser;
using AshyCore.Resource;

namespace AshyCore
{
    public class CoreAPI : IEngineAPI
    {
        public static EngineProxy I { get; private set; }

        public EngineStatus Preinitialize(EngineProxy baseEngine)
        {
            I                           = baseEngine;
            Engine.I                    = baseEngine.Core as Engine;
            if (Engine.I == null) 
                return                  ( EngineStatus.CriticalFailed );

            Engine.I.CommandHandler     = new CoreCommands.CoreCommandHandler();
                
            return                      ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Initialize()
        {
#if ! DEBUG
            try
            { 
#endif
                I.Core.Log              = new Log(I.UserName);
                I.Core.Log.Initialize   ();

                var iniParser           = new FileIniDataParser().ReadFile(I.UserName + ".ini");
                I.Core.UserConfig       = new ConfigTable(iniParser);

                if (I.Core.UserConfig["FS", "Type"] == "Basic")
                {
                    I.Core.FS           = new BasicFileSystem(I.Core.UserConfig["FS", "Path"]);
                }
                else if (I.Core.UserConfig["FS", "Type"] == "Zip")
                {
                    I.Core.FS           = new ZipFileSystem(I.Core.UserConfig["FS", "Path"]);
                }
                else
                {
                    throw               new Exception("Unknown FS type");
                }

                I.Core.RM               = new ResourceManager(I.Core.FS);
#if ! DEBUG
            }
            catch (Exception e)
            {
                //Critical.NoThrow(() => I.Core.Log.Error("AshyCore Initialization failed!", e));
                return                  ( EngineStatus.CriticalFailed );
            }
#endif
            I.Core.Log.Info             ("AshyCore: Initialization successful");
            return                      ( EngineStatus.ReadyToUse );
        }

        public EngineStatus Free()
        {
            Critical.NoThrow            ( () => Engine.I.FS.Dispose() );
            Critical.NoThrow            ( () => Engine.I.Log.End() );               // todo: check this
            int collectWaiting          = I.Core.RM.CollectWaiting(x => true);
            return                      collectWaiting == 0 
                                            ? ( EngineStatus.Free )
                                            : ( EngineStatus.FreeWithMemoryLeaks );
        }
    }
}

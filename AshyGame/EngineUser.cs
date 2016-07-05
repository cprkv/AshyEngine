// 
// Created : 25.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCore.EngineAPI;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyGame
{
    internal class EngineUser : EngineProxy
    {
        internal EngineUser(string[] cmdArgs)
        {
            Core                = new AshyCore      .Engine();
            Render              = new AshyRenderGL  .Engine();
            Script              = new AshyScripting .Engine();
            Physics             = new AshyPhysics   .Engine();
            Game                = new AshyGame      .Engine();

            CmdArgs             = cmdArgs;
            UserName            = cmdArgs.Length > 0 ? cmdArgs[0] : "UserDefault";
        }

        public void Initialize()
        {
            CommandProcessor    = new ProxyCommandProcessor(this);
        }

        public void Tick(float dtime)
        {
            Render.GameWindow.ProcessEvents();

            CommandProcessor.ProcessAllCommands();

            Core.Tick           ( dtime );
            Physics.Tick        ( dtime );
            Script.Tick         ( dtime );
            Game.Tick           ( dtime );

            Render.Tick         ( dtime );
        }
    }
}

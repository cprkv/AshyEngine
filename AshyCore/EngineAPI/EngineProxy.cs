// 
// Created : 26.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using AshyCore.Resource;
using AshyCore.VFS;

namespace AshyCore.EngineAPI
{
    public abstract class EngineProxy
    {
        public ICoreEngine Core { get; protected set; }

        public IRenderEngine Render { get; protected set; }

        public IScriptingEngine Script { get; protected set; }

        public IPhysicsEngine Physics { get; protected set; }

        public IGameEngine Game { get; protected set; }

        public string[] CmdArgs { get; protected set; }

        public string UserName { get; protected set; }

        public bool CheckAllPreinitialized =>
            Core    .Status >= EngineStatus.ReadyToLoad &&
            Render  .Status >= EngineStatus.ReadyToLoad &&
            Script  .Status >= EngineStatus.ReadyToLoad &&
            Physics .Status >= EngineStatus.ReadyToLoad &&
            Game    .Status >= EngineStatus.ReadyToLoad;

        public bool CheckAllInitialized =>
            Core    .Status == EngineStatus.ReadyToUse &&
            Render  .Status == EngineStatus.ReadyToUse &&
            Script  .Status == EngineStatus.ReadyToUse &&
            Physics .Status == EngineStatus.ReadyToUse &&
            Game    .Status == EngineStatus.ReadyToUse;
    }
}

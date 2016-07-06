// 
// Created : 10.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCore.EngineAPI.EngineCommands;

namespace AshyCore.EngineAPI
{
    public enum EngineStatus
    {
        CriticalFailed,
        Failed,
        ReadyToLoad,
        ReadyToUse,
        LoadedWorld,
        Free,
        FreeWithMemoryLeaks,
    }

    public interface IEngine
    {
        /// <summary>
        /// Simulation of engine through delta time.
        /// </summary>
        /// <param name="dtime">Time of last frame in ms.</param>
        void Tick(float dtime);

        IEngineCommandHandler CommandHandler { get; }

        EngineStatus Status { get; }
    }

    /// <summary>
    /// Description of the system module.
    /// </summary>
    public interface IEngineAPI
    {
        EngineStatus Preinitialize(EngineProxy baseEngine);

        EngineStatus Initialize();

        EngineStatus Free();
    }
}
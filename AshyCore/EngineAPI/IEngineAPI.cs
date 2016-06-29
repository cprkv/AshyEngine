// 
// Created : 10.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

namespace AshyCore.EngineAPI
{
    public enum EngineStatus
    {
        CriticalFailed,
        Failed,
        ReadyToLoad,
        ReadyToUse,
        Free,
        FreeWithMemoryLeaks,
    }

    public interface IEngine
    {
        void Tick(float dtime);

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
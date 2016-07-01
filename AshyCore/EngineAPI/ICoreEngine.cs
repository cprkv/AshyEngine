// 
// Created : 25.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCore.EngineAPI.EngineCommands;

namespace AshyCore.EngineAPI
{
    public interface ICoreEngine : IEngine
    {
        Resource.ResourceManager    RM { get; set; }

        VFS.IFileSystem             FS { get; set; }

        Log                         Log { get; set; }

        ConfigTable                 UserConfig { get; set; }
    }
}

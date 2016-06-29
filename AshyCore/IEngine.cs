// 
// Created : 17.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 
namespace AshyCore
{
    public interface IEngine
    {
        float GetLastFrameTime();

        void Exit();

        void EnterEditorMode();

        bool CommandLineEditor();
    }

    public interface IWorld
    {
        void Enable(bool value);

        void Tick(uint currentFrameId);

        void ClearResources();

        void Load(string projectName);

        void Unload(string wat);
    }

    public interface IModuleProxy
    {
        IWorld CreateWord(IEngine engine);

        void DestroyWorld(IWorld world);
    }
}
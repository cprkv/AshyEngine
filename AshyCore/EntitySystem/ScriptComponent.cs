//  
// Created  : 28.03.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCore.EntityManagement;
using AshyCore.Resource;

namespace AshyCore.EntitySystem
{
    /// <summary>
    /// Scripting component.
    /// </summary>
    /// <seealso cref="AshyCore.EntitySystem.IComponent" />
    public class ScriptComponent : IComponent
    {
        #region Properties

        public ComponentType Type { get; protected set; } = ComponentType.Script;

        public Script Script { get; private set; }

        #endregion


        #region Constructors

        public ScriptComponent(string scriptPath)
        {
            Script = CoreAPI.I.Core.RM.Get<Script>(
                "Scripts/" + scriptPath, 
                ResourceTarget.LoadedLevelPrivateScript);
        } 

        #endregion
    }
}
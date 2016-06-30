// 
// Created : 02.04.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System.Collections.Generic;
using AshyCore.EntityManagement;

namespace AshyCore.EngineAPI
{
    public interface IScriptingEngine : IEngine
    {
        /// <summary>
        /// Attaches the trigger to the specified <paramref name="zone"/>.
        /// </summary>
        ITrigger AttachTrigger(IZone zone, Script trigger, IEnumerable<EntitySystem.Entity> entities);

        /// <summary>
        /// Attaches the trigger to the specified <paramref name="zone"/> for only one <paramref name="e"/>.
        /// </summary>
        ITrigger AttachTrigger(IZone zone, Script trigger, EntitySystem.Entity e);
    }
}
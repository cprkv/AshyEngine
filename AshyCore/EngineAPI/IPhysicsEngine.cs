//  
// Created  : 27.03.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//   

using AshyCore.EntitySystem;

namespace AshyCore.EngineAPI
{
    public interface IPhysicsEngine : IEngine
    {
        /// <summary>
        /// Registers the character, which has unique phisics 
        /// and could be controlled from outside physics.
        /// </summary>
        /// <returns>Character physics controller.</returns>
        ICharacterPhysics RegisterCharacter(Entity e);
    }
}
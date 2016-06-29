//  
// Created  : 03.06.2016
// Author   : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//

using AshyCommon.Math;

namespace AshyCore.EntitySystem
{
    /// <summary>
    /// Describes entity inside physics, which could be controlled outside physics module.
    /// </summary>
    public interface ICharacterPhysics
    {
        /// <summary>
        /// Center of fictional physics object.
        /// </summary>
        Vec3 Center { get; }

        /// <summary>
        /// Manually sets the force vector of connected entity.
        /// </summary>
        /// <param name="a">Force vector.</param>
        void SetForce(Vec3 a);

        /// <summary>
        /// Manually stops the force of connected entity.
        /// </summary>
        void StopForce();
    }
}
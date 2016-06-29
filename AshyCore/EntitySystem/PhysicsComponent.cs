//  
// Created  : 28.03.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//

namespace AshyCore.EntitySystem
{
    public enum PhysicsType
    {
        Dynamic,
        Static,
    }

    public enum MotionType
    {
        Dynamic,
        Kinematic,
    }

    /// <summary>
    /// Physical simulation component.
    /// </summary>
    /// <seealso cref="AshyCore.EntitySystem.GeomComponent" />
    public class PhysicsComponent : GeomComponent
    {
        #region Properties

        public PhysicsType Physics { get; }

        public MotionType Motion { get; set; }

        #endregion


        #region Constructors

        public PhysicsComponent(Mesh mesh, PhysicsType physics, MotionType motion) : base(mesh)
        {
            Physics = physics;
            Type = ComponentType.Physics;
            Motion = motion;
        } 

        #endregion
    }
}
//  
// Created  : 28.03.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//

namespace AshyCore.EntitySystem
{
    /// <summary>
    /// 3D geometric component.
    /// </summary>
    /// <seealso cref="AshyCore.EntitySystem.IComponent" />
    public class GeomComponent : IComponent
    {
        #region Properties

        public ComponentType Type { get; protected set; } = ComponentType.Geom;

        public Mesh Mesh { get; }

        #endregion

        
        #region Constructors

        public GeomComponent(Mesh mesh)
        {
            Mesh = mesh;
        } 

        #endregion
    }
}
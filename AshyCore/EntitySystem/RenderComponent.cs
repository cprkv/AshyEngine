//  
// Created  : 28.03.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

namespace AshyCore.EntitySystem
{
    /// <summary>
    /// The component which contains information how to draw entity.
    /// </summary>
    /// <seealso cref="AshyCore.EntitySystem.GeomComponent" />
    public class RenderComponent : GeomComponent
    {
        #region Properties

        public Material Material { get; }

        #endregion


        #region Constructors

        public RenderComponent(Mesh mesh, Material material) : base(mesh)
        {
            Type = ComponentType.Render;
            Material = material;
        } 

        #endregion
    }
}
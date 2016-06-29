// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using AshyCommon;
using AshyCommon.Math;

namespace AshyCore.EntitySystem
{
    /// <summary>
    /// Class for objects which could be builded from entity components.
    /// Has a 3d space representation.
    /// </summary>
    public class Entity : ITransformBase
    {
        #region Fields

        private TransformBase _transform;

        public event Action<object, TransformBase> TransformEvent;

        #endregion


        #region Properties

        /// <summary>
        /// Components of entity.
        /// </summary>
        /// <remarks>Components is unique.</remarks> 
        private Dictionary<ComponentType, IComponent> Components { get; } = new Dictionary<ComponentType, IComponent>();

        public string Name { get; }
        
        /// <summary>
        /// Gets <see cref="IComponent"/> of the entity.
        /// </summary>
        /// <returns>If the entity has no specified component, returns <code>null</code>.</returns>
        public IComponent this[ComponentType type] => Components.GetOrNull(type);

        public Vec3 Position
        {
            get { return Transform.Position; }
            set { _transform.Position = value; OnTransformEvent(_transform); }
        } 

        public Quat Rotation
        {
            get { return Transform.Rotation; }
            set { _transform.Rotation = value; OnTransformEvent(_transform); }
        }

        public Vec3 Scale
        {
            get { return Transform.Scale; } 
            set { _transform.Scale = value; OnTransformEvent(_transform); }
        }

        public TransformBase Transform
        {
            get { return _transform; }
            set { _transform = value; OnTransformEvent(value); }
        }

        public TransformBase RelativeTo { get; set; }  = new TransformBase(Quat.Zero, Vec3.One, Vec3.Zero);

        public Mat4 TransformMatrix => RelativeTo.ResultMatrix * Transform.ResultMatrix;

        #endregion


        #region Constructors
        
        public Entity(string name, Vec3 position, Vec3 scale, Vec3 rotation)
        {
            Name = name;
            _transform = new TransformBase(rotation, scale, position);

#if DEBUG
            Log();
#endif
        }

        public Entity(Entity entity, string name, Vec3 position, Vec3 scale, Vec3 rotation)
        {
            Name = name;
            _transform = new TransformBase(rotation, scale, position);
            Components = entity.Components;

#if DEBUG
            Log();
#endif
        }

        #endregion


        #region Inline methods

        public bool HasComponent(ComponentType type)    => Components.ContainsKey(type);
        public void AddComponent(IComponent component)  => Components.GetOrAdd(component.Type, type => component);
        public void DelComponent(ComponentType type)    => Components.Remove(type);
        
        /// <summary>
        /// Sets transformation of the entity without happening <see cref="TransformEvent"/>.
        /// </summary>
        public void ForceSetTransform(TransformBase transform) => _transform = transform;

        #endregion


        #region Methods

        public TComponent Get<TComponent>(ComponentType type) where TComponent : class, IComponent
        {
            return Components.GetOrNull(type) as TComponent;
        }

        /// <summary>
        /// Print information about entity to log.
        /// </summary>
        public void Log()
        {
            var wat = $" Entity \"{Name}\":  pos={Position} rot={Rotation} scale={Scale}";
            CoreAPI.I.Core.Log.Info(wat);
        } 

        protected virtual void OnTransformEvent(TransformBase e)
        {
            TransformEvent?.Invoke(this, e);
        }

        #endregion
    }
}
// 
// Created : 25.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCommon.Math;
using AshyCore.Input;
using AshyCore.VFS;

namespace AshyCore.EntitySystem
{
    public class Player : Entity
    {
        #region Properties

        public Camera Camera { get; }

        public bool CameraAttached { get; set; } = false;

        private ICharacterPhysics CharPhysics { get; set; }

        #endregion


        #region Constructors

        public Player(Vec3 position, Vec3 scale, Vec3 rotation)
            : base("player", position, scale, rotation)
        {
            Camera = new Camera();
        }

        public void Register()
        {
            CoreAPI.I.Core.Log.Info("Registring player...");
            CharPhysics = CoreAPI.I.Physics.RegisterCharacter(this);
            //Engine.I.AddFrameIteration((sender, f) => Update()); !!
        }

        public void Update()
        {
            ///// UNCOMMENT MAZERFUCKER
            //if (CoreAPI.I.PressedKeys[(int)Key.C])
            //{
            //    CameraAttached = !CameraAttached;
            //}
            //if (!CameraAttached)
            //{
            //    //Camera.Dir = (Engine.Instance.Level.GetEntity("tree_0001").Position - Camera.Eye).Norm();

            //    var shiftTo = Vec2.Zero;
            //    if (CoreAPI.Instance.PressedKeys[(int)Key.W]) shiftTo.Y += 1f;
            //    if (CoreAPI.Instance.PressedKeys[(int)Key.S]) shiftTo.Y -= 1f;
            //    if (CoreAPI.Instance.PressedKeys[(int)Key.A]) shiftTo.X -= 1f;
            //    if (CoreAPI.Instance.PressedKeys[(int)Key.D]) shiftTo.X += 1f;
            //    Camera.Move(shiftTo);

            //    return;
            //}

            //Camera.Eye = CharPhysics.Center + new Vec3(0, 3, 0);
            //Vec3 moving = Vec3.Zero;

            //if (CoreAPI.Instance.PressedKeys[(int) Key.W])
            //{
            //    moving += Camera.Dir;
            //}
            //if (CoreAPI.Instance.PressedKeys[(int) Key.S])
            //{
            //    moving -= Camera.Dir;
            //}
            //if (CoreAPI.Instance.PressedKeys[(int) Key.D])
            //{
            //    moving += Camera.Right;
            //}
            //if (CoreAPI.Instance.PressedKeys[(int) Key.A])
            //{
            //    moving -= Camera.Right;
            //}

            //if (moving == Vec3.Zero)
            //{
            //    CharPhysics.StopForce();
            //}
            //else
            //{
            //    CharPhysics.SetForce(moving.Norm());
            //}

            // delete this
            //Engine.Instance.Level.GetEntity("hand_right_0001").Position = Position + new Vec3(0, 3, 0);
            //Engine.Instance.Level.GetEntity("hand_left_0001").Position  = Position + new Vec3(0, 3, 0);
            //Engine.Instance.Level.GetEntity("hand_left_0001").Rotation  = Camera.View.ToQuat().Conj.Normalize();
            //Engine.Instance.Level.GetEntity("hand_right_0001").Rotation = Camera.View.ToQuat().Conj.Normalize();

        }

        #endregion
    }
}
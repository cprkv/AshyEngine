// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AshyCommon;
using AshyCommon.Math;
using AshyCore.EntityManagement;
using AshyCore.EntitySystem;

namespace AshyCore
{
    // todo: move realization to game
    public class GameLevel
    {
        #region Properties

        public List<Entity>                 Entities;

        public Player                       Player;

        public string                       Name { get; }

        public ConfigTable                  LevelInfo { get; }

        public Dictionary<string, IZone>    Zones { get; private set; }

        public Dictionary<string, ITrigger> Triggers { get; private set; }

        #endregion


        #region Constructors

        public GameLevel(string name, ConfigTable levelInfo)
        {
            Name = name;
            LevelInfo = levelInfo;
        }

        #endregion


        #region Public methods

        public void Load()
        {
            Entities = LevelInfo["Entities"]
                .Select(key => key.Key)
                .Select(LoadEntity)
                .ToList();
            Player = LoadPlayer();
            Entities.Add(Player);
            LoadZones();
            LoadTriggers();
            CoreAPI.I.Core.Log.Info($"[Engine] Level {Name} loaded.");
        }

        public async void LoadInModulesAsync()
        {
            await Task.Run(() =>
            {
                //CoreAPI.Instance.Modules.ForEach(m => m.LoadLevel(this)); !!
            });
        }

        public IEnumerable<Entity> GetEntities(ComponentType type)
        {
            return Entities.Where(entity => entity.HasComponent(type));
        }

        public IEnumerable<Entity> GetEntities(IEnumerable<string> names)
        {
            return Entities.Where(entity => names.Contains(entity.Name));
        }

        public Entity GetEntity(string name)
        {
            return Entities.FirstOrDefault(x => x.Name == name);
        }

        public void Spawn(Entity entity)
        {
            Entities.Add(entity);
            //CoreAPI.Instance.Modules.ForEach(m => m.RegisterEntity(entity)); !!
        }

        #endregion


        #region Private methods

        private Vec3 ReadVec3(string key, string value) => Vec3.Parse(LevelInfo[key, value]);

        private Player LoadPlayer()
        {
            var e = new Player(
                ReadVec3("Player", "Position"),
                ReadVec3("Player", "Scale"),
                ReadVec3("Player", "Rotation")
                );
            return e;
        }

        private Entity LoadEntity(string entityName)
        {
            // todo move to parser
            var e = new Entity(
                entityName,
                ReadVec3(entityName, "Position"),
                ReadVec3(entityName, "Scale"),
                ReadVec3(entityName, "Rotation")
                );
            string scriptComponent  = LevelInfo[e.Name, "ScriptComponent"];
            string renderComponent  = LevelInfo[e.Name, "RenderComponent"];
            string geomComponent    = LevelInfo[e.Name, "GeomComponent"];
            string physicsComponent = LevelInfo[e.Name, "PhysicsComponent"];

            if (scriptComponent != null)
            {
                e.AddComponent(new ScriptComponent(LevelInfo[scriptComponent, "ScriptPath"]));
            }
            if (geomComponent != null)
            {
                var mesh = CoreAPI.I.Core.RM.Get<Mesh>(
                    $"Meshes/{LevelInfo[geomComponent, "Mesh"]}", 
                    Resource.ResourceTarget.LoadedLevel
                    );
                e.AddComponent(new GeomComponent(mesh));

                if (renderComponent != null)
                {
                    e.AddComponent(new RenderComponent(mesh, new Material(
                        ReadVec3(renderComponent,  "Color"),
                        LevelInfo[renderComponent, "Shader"],
                        LevelInfo[renderComponent, "Diffuse"],
                        LevelInfo[renderComponent, "Normal"])));
                }
                if (physicsComponent != null)
                {
                    e.AddComponent(new PhysicsComponent(mesh, 
                        LevelInfo[physicsComponent, "PhysicsType"].AsEnum<PhysicsType>(),
                        LevelInfo[physicsComponent, "MotionType"].AsEnum<MotionType>()));
                }
            }
            return e;
        }

        private void LoadZones()
        {
            // todo move to game
            Zones = LevelInfo["Zones"].ToDictionary(x => x.Key, x =>
            {
                IZone zone;
                if (x.Value == "Spherical")
                {
                    zone = new SphericalZone(ReadVec3(x.Key, "Center"), LevelInfo[x.Key, "Radius"].AsFloat().Value);
                }
                else
                {
                    var aab = ReadVec3(x.Key, "AAB");
                    zone = new AABZone(ReadVec3(x.Key, "Center"), aab.X, aab.Z, aab.Z);
                }
                return zone;
            });
        }

        private void LoadTriggers()
        {
            // todo move to game
            Triggers = LevelInfo["Triggers"].ToDictionary(x => x.Key, x =>
                CoreAPI.I.Script.AttachTrigger(
                    Zones[LevelInfo[x.Key, "Zone"]],
                    CoreAPI.I.Core.RM.Get<Script>(
                        $"Scripts/{LevelInfo[x.Key, "Script"]}",
                        Resource.ResourceTarget.LoadedLevel),
                    GetEntities(LevelInfo[x.Key, "Entities"].Split())
                    )
                );
        }

        #endregion
    }
}
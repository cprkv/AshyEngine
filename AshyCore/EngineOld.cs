// 
// Created : 11.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AshyCommon;
using AshyCore.Resource;
using IniParser;

namespace AshyCore
{
    /// <summary>
    /// The class combines all modules of the engine and runs it.
    /// </summary>
    public class EngineOld // : EngineAPI.IEngineAPI
    {
        #region Properties

        //public bool IsStarted { get; private set; } = false;

        //public bool[] PressedKeys { get; set; } = new bool[131];

        #endregion


        #region Constructors and destructors

        //private Engine()
        //{

        //}

        #endregion


        #region Public Methods

        ///// <summary>
        ///// Loading all modules and preparing engine to run.
        ///// </summary>
        //public void Load(string[] args)
        //{
        //    string engineUser = args.Length > 0 ? args[0] : "UserDefault";
        //    var userConfig = $"{engineUser}.ini";

        //    Log = new Log(engineUser);

        //    UserConfig  = new ConfigTable(new FileIniDataParser().ReadFile(userConfig));

        //    Log.Info($"[Engine] Loaded user config {userConfig}");

        //    FS          = new BasicFileSystem(UserConfig["Engine", "Path"]);
        //    Resource    = new ResourceManager(FS);

        //    var gameInfo = Resource.Get<ConfigTable>("Config/GameInfo", ResourceTarget.Engine);

        //    LoadWorld(gameInfo);
        //    LoadModules(gameInfo);

        //    AddFrameIteration((sender, dtime) => Modules.ForEach(m => m.Update(dtime)));
        //}

        ///// <summary>
        ///// Entry into the "infinite" loop, break by the setting of values 'false'
        ///// field <see cref="IsRunning"/>.
        ///// </summary>
        //public void EnterMainLoop()
        //{
        //    Modules.ForEach(m => m.Start());
        //    Level.Load();
        //    Level.LoadInModulesAsync();
        //    Level.Player.Register();


        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
        //    Process currentProc = Process.GetCurrentProcess();
        //    Log.Info($"[Engine] Сollecting memory. End status: {currentProc.PrivateMemorySize64/1024/1024} Mb");
        //    Log.Info("[Engine] All systems were loaded.");
        //    IsStarted = true;
        //    Render.Start();
        //}

        //public void Stop()
        //{
        //    Modules.ForEach(m => m.End());
        //    Render.End();
        //    FS.Dispose();
        //    Log.End();
        //}

        //public void AddFrameIteration(EventHandler<float> func)
        //{
        //    if (func != null) Render.FrameIteration += func;
        //}

        //#endregion


        //#region Private methods

        //private void LoadModules(ConfigTable gameInfo)
        //{
        //    var loadingModules = gameInfo["Game", "Modules"]
        //        .Split()
        //        .Select(m => AppDomain.CurrentDomain.BaseDirectory + m);

        //    foreach (string module in loadingModules)
        //    {
        //        try
        //        {
        //            var instances = AssemblyUtils.Load<IModule>(module);
        //            // for each ( this.propertyes : IModule ) do { property = suitable module }
        //            instances
        //                .ToDictionary(m => m, m => typeof(Engine).GetProperties().FirstOrDefault(t => t.Name == m.Name))
        //                .ForEach(info => info.Value?.SetMethod.Invoke(this, new object[] { info.Key }));
        //            Modules.AddRange(instances);
        //        }
        //        catch (Exception)
        //        {
        //            Log.Error($"Can't load module {module}");
        //        }
        //    }

        //    Render = AssemblyUtils.Load<IRenderModule>(
        //        AppDomain.CurrentDomain.BaseDirectory + UserConfig["Engine", "Render"]
        //        ).FirstOrDefault();
        //}

        //private void LoadWorld(ConfigTable gameInfo)
        //{
        //    var worldInfo = gameInfo["World"];

        //    List<GameLevel> levels =
        //        (from levelName in worldInfo["Levels"].Split()
        //         let levelInfo = Resource.Get<ConfigTable>($"Config/Levels/{levelName}", ResourceTarget.World)
        //         select new GameLevel(levelName, levelInfo)
        //         ).ToList();

        //    World = new World(levels);
        //    Level = World.Levels.FirstOrDefault(x => x.Name == UserConfig["Developer", "StartLevel"]);

        //    if (Level == null)
        //    {
        //        throw new Exception($"Can't load default level \"{UserConfig["Developer", "StartLevel"]}\"");
        //    }
        //}

        #endregion
    }
}
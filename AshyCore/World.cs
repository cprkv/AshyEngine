// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System.Collections.Generic;

namespace AshyCore
{
    public class World
    {
        #region Properties

        public List<GameLevel> Levels { get; }

        #endregion

        
        #region Constructors

        public World(List<GameLevel> levels)
        {
            Levels = levels;
        }

        #endregion
    }
}
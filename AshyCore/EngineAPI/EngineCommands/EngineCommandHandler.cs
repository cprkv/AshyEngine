//   
// Created : 03.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;
using System.Collections.Generic;

namespace AshyCore.EngineAPI.EngineCommands
{
    public enum EngineCommandResult
    {
        Success,
        Failed,
        CriticalFailed,
    }

    public interface IEngineCommandHandler
    {
        EngineCommandResult     Execute(IEngineCommand c);
    }


    public class EngineCommandHandlerBase : IEngineCommandHandler
    {
        /// <summary>
        /// User should create it.
        /// If command is not set, method <see cref="Execute"/> 
        /// returns <code>EngineCommandResult.Success</code>.
        /// </summary>
        protected Dictionary<EngineCommandType, IEngineCommandHandler> Executers { get; set; } = null;

        public EngineCommandResult Execute(IEngineCommand c)
        {
            if (Executers == null) 
                throw new NullReferenceException(nameof(c));

            IEngineCommandHandler executer;
            Executers.TryGetValue       ( c.Type, out executer );

            return                      ( executer == null ? EngineCommandResult.Success : Execute(c) );
        }
    }
}
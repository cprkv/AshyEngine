// 
// Created : 18.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Linq;
using IniParser.Model;

namespace AshyCore
{
    /// <summary>
    /// Abstraction for ini parser.
    /// </summary>
    public class ConfigTable : Dictionary<string, Dictionary<string, string>>
    {
        #region Properties

        /// <summary>
        /// Indexer alias for feel comfortable.
        /// </summary>
        public string this[string section, string key] => 
            ContainsKey(section) && this[section].ContainsKey(key) ? this[section][key] : null;

        #endregion
        
        
        #region Constructors

        public ConfigTable(IniData data)
        {
            foreach (var section in data.Sections)
            {
                var tempSection = section.Keys
                    .ToDictionary(configLine => configLine.KeyName, configLine => configLine.Value);
                Add(section.SectionName, tempSection);
            }
        }

        #endregion


        #region Methods

        /// <summary>
        /// Saves configuration to file <paramref name="filepath"/>.
        /// </summary>
        public void Save(string filepath)
        {
            throw new NotImplementedException();
        } 

        #endregion
    }
}

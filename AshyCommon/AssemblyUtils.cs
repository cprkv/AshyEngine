// 
// Created : 13.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AshyCommon
{
    /// <summary>
    /// Helper-class for working with c# assemblyies.
    /// </summary>
    public static class AssemblyUtils
    {
        #region Private fields

        private static int? _buildNumber;

        #endregion


        #region Public properties

        /// <summary>
        /// Project's start day.
        /// </summary>
        public static DateTime FirstBuildDate => new DateTime(2016, 3, 10);

        /// <summary>
        /// Gets the build number (days scince project started).
        /// </summary>
        public static int BuildNumber
        {
            get
            {
                if (_buildNumber == null) _buildNumber = GetBuildNumber();
                return _buildNumber.Value;
            }
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Loads the assembly extracts it from the heir 'TFind'
        /// (one, if it finds a few - will take the first available),
        /// and returns the constructed instance.
        /// </summary>
        /// <param name="path">The path to the 'dll' library.</param>
        /// <remarks>Constructor found a class TFind must be empty!</remarks>
        public static List<TFind> Load<TFind>(string path)
        {
            var lib = Assembly.LoadFile(path);

            var foundTypes = lib
                .GetExportedTypes()
                .Where(x => x.IsClass && x.GetInterfaces().Any(t => t == typeof (TFind)))
                .ToList();

            if (foundTypes.Count == 0)
            {
                throw new Exception($"Can't find any realisation of {typeof(TFind).Name} " +
                                    $"inside assembly {path}.");
            }

            var instances = foundTypes
                .Select(x => (TFind)x.GetConstructor(new Type[] { })?.Invoke(null))
                .Where(x => x != null)
                .ToList();

            if (instances.Count == 0)
            {
                throw new Exception($"Can't find any realisation of {typeof(TFind).Name} " +
                                    $"inside assembly {path} with empty constructor.");
            }

            return instances;
        }

        public static List<Type> FindTypesByBaseType(this Assembly assembly, Type baseType)
        {
            return assembly
                .GetTypes()
                .Where(x => x.BaseType == baseType)
                .ToList();
        }

        public static List<Type> FindTypesByInterfaceType(this Assembly assembly, Type baseType)
        {
            return assembly
                .GetTypes()
                .Where(x => x.GetInterfaces().Any(y => y == baseType))
                .ToList();
        }

        /// <summary>
        /// Creates the object of class which is inherited from <see cref="baseType"/>.
        /// Class type should match the predicate and have public empty constructor.
        /// </summary>
        /// <returns>Constructed instance.</returns>
        public static object CreateObjectByBaseType(Assembly assembly, Type baseType, Func<Type, bool> predicate, List<Type> searchedTypes = null)
        {
            searchedTypes = searchedTypes ?? FindTypesByBaseType(assembly, baseType);
            return searchedTypes
                .FirstOrDefault(predicate)
                ?.GetConstructor(new Type[] {})
                ?.Invoke(new object[] {});
        }

        /// <summary>
        /// Creates the object of class which is inherited from <see cref="baseType"/>.
        /// Class type should match the predicate and have public empty constructor.
        /// </summary>
        /// <returns>Constructed instance.</returns>
        public static IEnumerable<object> CreateObjectsByBaseType(Assembly assembly, Type baseType, Func<Type, bool> predicate, List<Type> searchedTypes = null)
        {
            searchedTypes = searchedTypes ?? FindTypesByBaseType(assembly, baseType);
            return searchedTypes
                .Where(predicate)
                .Select(t => t.GetConstructor(new Type[] {}))
                .Select(c => c.Invoke(new object[] {}));
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Gets linker time.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <remarks>Cost method.</remarks>
        private static DateTime GetLinkerTime(this Assembly assembly)
        {
            var filePath = assembly.Location;
            const int cPeHeaderOffset = 60;
            const int timestampOffset = 8;
            var buffer = new byte[2048];
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);
            var offset = BitConverter.ToInt32(buffer, cPeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + timestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, TimeZoneInfo.Local);

            return localTime;
        }

        /// <summary>
        /// Calculates build number. (days scince project started).
        /// </summary>
        private static int GetBuildNumber()
        {
            var linkTimeLocal = Assembly.GetExecutingAssembly().GetLinkerTime();
            return (int)(linkTimeLocal - FirstBuildDate).TotalDays;
        }

        #endregion 
    }
}
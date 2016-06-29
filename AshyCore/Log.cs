// 
// Created : 13.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AshyCommon;

namespace AshyCore
{
    /// <summary>
    /// Event logger class.
    /// </summary>
    public class Log 
    {
        #region Properties

        /// <summary>
        /// Stream writer in log-file.
        /// </summary>
        private StreamWriter Writer { get; }

        /// <summary>
        /// All the log that is written to the file is duplicated this list, to display to the user on the screen.
        /// </summary>
        private List<string> WriterList { get; }

        #endregion


        #region Constructors

        /// <summary>
        /// Automatically create file like a "AshyEngine_дата_время.txt", opens a stream for writing.
        /// Writes the initial information.
        /// </summary>
        internal Log(string engineUser)
        {
            var dateTime =
                DateTime.Now.ToString(CultureInfo.InvariantCulture)
                    .Replace('/', '.')
                    .Replace(':', '-')
                    .Replace(' ', '_');
            var path = $"Log/{engineUser}/AshyEngine_{dateTime}.log";
            try
            {
                Writer = new StreamWriter(path);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory($"Log/{engineUser}");
                Writer = new StreamWriter(path);
            }
            WriterList = new List<string>();
        }

        #endregion


        #region Methods

        /// <summary>
        /// Write to log information.
        /// </summary>
        public void Info(string wat)
        {
            var dateTime = DateTime.Now;
            var format = $"{dateTime.ToShortDateString()} {dateTime.ToShortTimeString()}: {wat}";
            WriteLogString(format);
        }

        internal void Initialize()
        {
            Info($"[AshyEngine build {AssemblyUtils.BuildNumber}]");
        }

        /// <summary>
        /// Write to log error (prefix "[ERROR]")
        /// </summary>
        public void Error(string wat)
        {
            var dateTime = DateTime.Now;
            var format = $"{dateTime.ToShortDateString()} {dateTime.ToShortTimeString()} [ERROR]: {wat}";
            WriteLogString(format);
        }

        /// <summary>
        /// Write to log error (prefix "[ERROR]")
        /// </summary>
        public void Error(string wat, Exception e)
        {
            Error(wat);
            Error("--- Reason: " + e.Message);
            Error("--- StackTrace: " + e.StackTrace);
        }

        public void End()
        {
            Writer.Close();
        }

        private void WriteLogString(string format)
        {
            Writer.WriteLine(format);
            WriterList.Add(format);
            Console.WriteLine(format);
        }

        #endregion
    }
}
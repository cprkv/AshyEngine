// 
// Created : 02.04.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using AshyCommon;

namespace AshyCore
{
    public class ShaderAlias : IEquatable<ShaderAlias>
    {
        #region Properties
        
        /// <summary>
        /// Vertex shader path
        /// </summary>
        public string VertexShader { get; }

        /// <summary>
        /// Fragment shader path
        /// </summary>
        public string FragmentShader { get; }

        /// <summary>
        /// Uniform variables: variable name -> type
        /// </summary>
        public Dictionary<string, string> Uniform { get; }

        #endregion


        #region Constructors

        public ShaderAlias(string vertexShader, string fragmentShader, Dictionary<string, string> uniform)
        {
            VertexShader = vertexShader;
            FragmentShader = fragmentShader;
            Uniform = uniform;
        }

        #endregion


        #region Equality methods

        public bool Equals(ShaderAlias other)
        {
            return VertexShader == other.VertexShader
                && FragmentShader == other.FragmentShader
                && Uniform.Eq(other.Uniform);
        }

        public static bool operator ==(ShaderAlias a, ShaderAlias b)
        {
            return ((object)a == null && (object)b == null)
                || ((object)a != null && (object)b != null && a.Equals(b));
        }

        public static bool operator !=(ShaderAlias a, ShaderAlias b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ShaderAlias)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = VertexShader?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (FragmentShader?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Uniform?.GetHashCode() ?? 0);
                return hashCode;
            }
        } 

        #endregion
    }
}
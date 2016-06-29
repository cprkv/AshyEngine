//  
// Created  : 11.06.2016
// Author   : Compiles
// © AshyCat, 2016 
//  

using System;
using System.Collections.Generic;
using System.Linq;

namespace AshyCommon.Math
{
    public class BezierCurve
    {
        public struct Segment
        {
            public Vec3 A;
            public Vec3 B;

            public Vec3 ControlA;
            public Vec3 ControlB;

            public Segment(Vec3 a, Vec3 b, Vec3 controlA, Vec3 controlB)
            {
                A = a;
                B = b;
                ControlA = controlA;
                ControlB = controlB;
            }

            //public Segment(Vec3 a, Vec3 b, Vec3 dirA, Vec3 dirB)
            //{
            //    A = a;
            //    B = b;
            //    ControlA = dirA + A;
            //    ControlB = dirB + B;
            //}

            public Vec3 Interpolate(float t)
            {
                var u = Vec3.Lerp(A, ControlA, t);
                var v = Vec3.Lerp(ControlA, ControlB, t);
                var w = Vec3.Lerp(ControlB, B, t);
                var m = Vec3.Lerp(u, v, t);
                var n = Vec3.Lerp(v, w, t);
                 return Vec3.Lerp(m, n, t);
            }

            public float Length(uint n)
            {
                var points = Defuse(n);
                var previous = points.FirstOrDefault();

                return Defuse(n).Aggregate(0.0f, (sum, point) =>
                {
                    var ret = (point - previous).Len;
                    previous = point;
                    return ret + sum;
                });
            }

            public IEnumerable<Vec3> Defuse(uint n)
            {
                float v = 1.0f / (n - 1);

                for (float i = 0; i <= 1; i += v)
                {
                    yield return Interpolate(i);
                }
            }
        }

        List<Segment> _segments;
        public float Length { get; }
        public float[] SegmentsLength { get; }

        public BezierCurve(List<Segment> segments, uint n)
        {
            _segments = segments;
            Length = _segments.Aggregate(0.0f, (sum, segment) => sum + segment.Length(n));
            SegmentsLength = _segments.Select(segment => segment.Length(n)).ToArray();
        }

        public Vec3 Interpolate(float t)
        {
            if (t <= 0)
                return _segments.First().A;
            if (t >= 1)
                return _segments.Last().B;
            var x = 0.0f.Lerp(Length, t);
            var y = 0.0f;
            var i = 0;
            while (y < x)
            {
                y += SegmentsLength[i];
                i++;
            }
            i--;
            y -= SegmentsLength[i];
            return _segments[i].Interpolate((x - y) / SegmentsLength[i]);
        }
    }
}
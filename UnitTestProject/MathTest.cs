using System.Runtime.InteropServices;
using System.Security.AccessControl;
using AshyCommon.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace UnitTestProject
{
    [TestClass]
    public class MathTest
    {
        [TestMethod]
        public void Vector2Test()
        {
            Vec2 vec2A = Vec2.Zero;
            vec2A.Add(new Vec2(1f, 1f));
            vec2A.Add(new Vec2(3f, 2f));
            Assert.AreEqual(vec2A, new Vec2(4f, 3f));

            var vec2B = (vec2A + new Vec2(0f, 2f) * 0.5f) / 2 - new Vec2(2f, 2f);
            Assert.AreEqual(vec2B, Vec2.Zero);

            var vec2C = Vec2.Zero.Add(new Vec2(3, 4));
            Assert.AreEqual(vec2C, new Vec2(3, 4));
            Assert.AreEqual(vec2C.LenSqr, 25);
            Assert.AreEqual(vec2C.Len, 5);
            Assert.AreEqual(Vec2.Zero, new Vec2(0, 0));


            vec2C.NormSelf();
            Assert.AreEqual(vec2C, new Vec2(3f, 4f) / 5f);      // check normself
            Assert.AreEqual(vec2C.Len, 1);                      // check length after normself
        }

        [TestMethod]
        public void Vector3Test()
        {
            Vec3 vec3A = Vec3.Zero;                             // (0, 0, 0) +
            vec3A.Add(new Vec3(1f, 1f, 2f));                    // (1, 1, 2) * 3
            vec3A.Mul(3f);                                      // (3, 3, 6) - (3, 2, 1)
            vec3A.Sub(new Vec3(new Vec2(3f, 2f), 1f));          // (0, 1, 5)

            Assert.AreEqual(vec3A, new Vec3(0f, 1f, 5f));

            vec3A.Add(new Vec3(5, 4, 0));                       // (5, 5, 5) /
            vec3A.Div(new Vec3(5, 5, 5));                       // (5, 5, 5)

            Assert.AreEqual(vec3A, Vec3.One);                   // ((0, 1, 5) + (5, 4, 0)) / (5, 5, 5) =  (1, 1, 1)
            Assert.AreEqual(vec3A.Clip(), new Vec2(1, 1));      // clipping

            Assert.AreEqual(Vec3.UnitX.Dot(Vec3.UnitY), 0);     // dot product

        }

        [TestMethod]
        public void MatrixTest()
        {
            var a = new Vec4(1, 2, -5, 4);
            // filling matrix with rows
            var b = new Mat4(
                new Vec4(-1, 4, 3, 1),
                new Vec4(1, 3, -1, -2),
                new Vec4(3, 32, 13, 3),
                new Vec4(2, 3, -2, 3)
                );
            // filling matrix with columns and transposing
            var b1 = new Mat4(new float[] { -1, 4, 3, 1, 1, 3, -1, -2, 3, 32, 13, 3, 2, 3, -2, 3 }).Transpose();

            Assert.AreEqual(a * b, new Vec4(-4, 4, 14, 30));
            Assert.AreEqual(a * b1, new Vec4(-4, 4, 14, 30));

            var mat = new Mat4(
                new Vec4(-4, 1, 2, 4),
                new Vec4(0, 1, -5, -1),
                new Vec4(5, 0, 12, -1),
                new Vec4(4, 1, 2, 1)
                ).Mul(b);

            Assert.AreEqual(mat, new Mat4(
                new Vec4(23, 4, 16, -10),
                new Vec4(-17, 2, -29, 0),
                new Vec4(65, 38, 8, -30),
                new Vec4(-6, 8, -29, 10)));

            Assert.AreEqual(b.Invert().Transpose(), new Mat4(new Vec4(-1.188889f, -0.5111111f, 0.2111111f, -0.1555556f), // inversion + transpose
                                                             new Vec4(0.3111111f, 0.2388889f, -0.03888889f, 0.09444445f),
                                                             new Vec4(-0.5222222f, -0.4277778f, 0.1277778f, -0.2388889f),
                                                             new Vec4(0.1333333f, -0.1833333f, -0.01666667f, 0.1833334f)));
        }

        [TestMethod]
        public void Vector4Test()
        {
            Vec4 vect = Vec4.Zero;
            vect.Add(new Vec4(1f, 1f, 2f, 2f)); //  1  1 2 2
            vect.Sub(new Vec4(3f, 2f, 1f, 2f)); // -2 -1 1 0

            Assert.AreEqual(vect, new Vec4(-2f, -1f, 1f, 0f));
        }

        [TestMethod]
        public void CommonMathTest()
        {
            Assert.AreEqual(4f.Sqrt(), 2);
            Assert.AreEqual(2f.Sqr(), 4);
            Assert.AreEqual((-4f).Abs(), 4);
            Assert.AreEqual(0f.Sin(), 0);
            Assert.AreEqual(0f.Cos(), 1);
            Assert.AreEqual(0f.Acos().ToDegrees(), 90f);
            Assert.AreEqual(0f.Asin().ToDegrees(), 0f);
            Assert.AreEqual(Math.Atan2(1, 2), 0.4636476f);
            Assert.AreEqual(180f.ToRadians(), Math.Pi);
            Assert.AreEqual(Math.Pi.ToDegrees(), 180f);
        }
    }
}

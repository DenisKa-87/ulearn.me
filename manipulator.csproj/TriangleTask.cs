using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        /// <summary>
        /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
        /// </summary>
        public static double GetABAngle(double a, double b, double c)
        {
            if (a < 0 || b < 0 || c < 0) return double.NaN;
            if ((a + b <= c || a + c <= b || b + c <= a) && (a > 0 && b > 0 && c > 0)) return double.NaN;
            double angleAB = Math.Acos((c * c - a * a - b * b) / (-2 * a * b));
            //double angleAB = Math.Asin(c*Math.Sin(angleBC)/a);
            //double angleAC = Math.PI - angleAB - angleBC;
            return angleAB;
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(1, 1, 1, Math.PI / 3)]
        [TestCase(1, 5, 0, double.NaN)]
        [TestCase(1, 4, 9, double.NaN)]

        // добавьте ещё тестовых случаев!
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            var angle = TriangleTask.GetABAngle(a, b, c);
            Assert.AreEqual(expectedAngle, angle, 1e-05);
        }
    }
}
using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            
            
            var elbowPos = new PointF((float)(Manipulator.UpperArm * Math.Cos((shoulder))),
                (float)(Manipulator.UpperArm * Math.Sin((shoulder))));
            var wristPos = new PointF(elbowPos.X + (float)(Manipulator.Forearm * Math.Cos(shoulder + Math.PI + elbow)),
                elbowPos.Y + (float) (Manipulator.Forearm * Math.Sin(shoulder + Math.PI + elbow))); ;
            var palmEndPos = new PointF(wristPos.X 
                + (float) (Manipulator.Palm * Math.Cos(shoulder + Math.PI + elbow + Math.PI + wrist)), 
                wristPos.Y + (float) (Manipulator.Palm * Math.Sin(shoulder + Math.PI + elbow + Math.PI + wrist)));
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        // Доработайте эти тесты!
        // С помощью строчки TestCase можно добавлять новые тестовые данные.
        // Аргументы TestCase превратятся в аргументы метода.
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(0, Math.PI , Math.PI, Manipulator.Forearm + Manipulator.Palm + Manipulator.UpperArm, 0)]
        [TestCase(Math.PI, Math.PI/2, Math.PI/2, Manipulator.Palm - Manipulator.UpperArm, Manipulator.Forearm)]
        [TestCase(Math.PI/2, Math.PI, Math.PI,  0, Manipulator.Forearm + Manipulator.Palm + Manipulator.UpperArm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
            //Assert.Fail("TODO: проверить, что расстояния между суставами равны длинам сегментов манипулятора!");
        }
        
        /*[Test]
        [TestCase(1.5707963267949d, 1.5707963267949d, 3.14159265358979d, Manipulator.UpperArm, Manipulator.Forearm, Manipulator.Palm)]

        public void TestJointsLength (double shoulder, double elbow, double wrist, double upperArm, double forearm, double palm)
        {
            //upper arm length
            var points = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            var upperArmX = points[0].X;
            var upperArmY = points[0].Y;
            var upperArmLen = (Math.Sqrt(upperArmX * upperArmX + upperArmY * upperArmY));
            //forearm length
            var forearmX = points[1].X;
            var forearmY = points[1].Y;
            var forearmLen = Math.Sqrt(Math.Pow((forearmX - upperArmX), 2) + Math.Pow((forearmY - upperArmY), 2));
            //palm length
            var palmX = points[2].X;
            var palmY = points[2].Y;
            var palmLen = Math.Sqrt(Math.Pow((palmX - forearmX), 2) + Math.Pow((palmY - forearmY), 2));

            Assert.AreEqual(upperArm, upperArmLen, 1e-3, "upper arm length");
            Assert.AreEqual(forearm, forearmLen, 1e-3, "forearm length");
            Assert.AreEqual(palm, palmLen, 1e-3, "palm length");
            //Assert.Fail("segments' lenghts are not equal!");

        }*/
    }
}
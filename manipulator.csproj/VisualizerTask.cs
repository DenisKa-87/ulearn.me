using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Manipulation
{
	public static class VisualizerTask
	{
		public static double X = 220;
		public static double Y = -100;
		public static double Alpha = 0.05;
		public static double Wrist = 2 * Math.PI / 3;
		public static double Elbow = 3 * Math.PI / 4;
		public static double Shoulder = Math.PI / 2;
		public static Brush UnreachableAreaBrush = new SolidBrush(Color.FromArgb(255, 255, 230, 230));
		public static Brush ReachableAreaBrush = new SolidBrush(Color.FromArgb(255, 230, 255, 230));
		public static Pen ManipulatorPen = new Pen(Color.Black, 3);
		public static Brush JointBrush = Brushes.Gray;

		public static void KeyDown(Form form, KeyEventArgs key)
		{
			if (key.KeyValue == 81) Shoulder += Math.PI / 18; 
            else if (key.KeyValue == 65) Shoulder -= Math.PI / 18;
			else if (key.KeyValue == 87) Elbow += Math.PI / 18;
			else if (key.KeyValue == 83) Elbow -= Math.PI / 18;
            Wrist = -Alpha - Shoulder - Elbow;
			// TODO: Добавьте реакцию на QAWS и пересчитывать Wrist
            form.Invalidate();
        }

		public static void MouseMove(Form form, MouseEventArgs e)
		{
            var mouseClick = new PointF (e.X, e.Y);
			var shoulderPos = GetShoulderPos(form);
			X = ConvertWindowToMath(mouseClick, shoulderPos).X;
			Y = ConvertWindowToMath(mouseClick, shoulderPos).Y;
			// TODO: Измените X и Y пересчитав координаты (e.X, e.Y) в логические.
            UpdateManipulator();
			form.Invalidate();
		}

		public static void MouseWheel(Form form, MouseEventArgs e)
		{
			// TODO: Измените Alpha, используя e.Delta — размер прокрутки колеса мыши
			Alpha += e.Delta;
			UpdateManipulator();
			form.Invalidate();
		}

		public static void UpdateManipulator()
		{
			if (Shoulder != double.NaN && Elbow != double.NaN && Wrist != double.NaN) 
				ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
			// Вызовите ManipulatorTask.MoveManipulatorTo и обновите значения полей Shoulder, Elbow и Wrist, 
            // если они не NaN. Это понадобится для последней задачи.
		}

		public static void DrawManipulator(Graphics graphics, PointF shoulderPos)
		{
			var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);
			graphics.DrawString(
                $"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}", 
                new Font(SystemFonts.DefaultFont.FontFamily, 12), Brushes.DarkRed, 10, 10);
			DrawReachableZone(graphics, ReachableAreaBrush, UnreachableAreaBrush,
				shoulderPos, joints);
			DrawLimbsAndJoints(graphics, shoulderPos, joints);
        }

		public static void DrawLimbsAndJoints(Graphics graphics, PointF shoulderPos, PointF[] joints)
		{
            int jointDiameter = 20;
            for (int i = 0; i < joints.Length; i++)
            {
                var XWindow = ConvertMathToWindow(joints[i], shoulderPos).X;
                var YWindow = ConvertMathToWindow(joints[i], shoulderPos).Y;
                graphics.FillEllipse(JointBrush, XWindow - jointDiameter / 2, YWindow - jointDiameter / 2,
                jointDiameter, jointDiameter);
                if (i == 0)
                {
                    graphics.FillEllipse(JointBrush, shoulderPos.X - jointDiameter / 2, 
						shoulderPos.Y - jointDiameter / 2,
                        jointDiameter, jointDiameter);
                    graphics.DrawLine(ManipulatorPen, shoulderPos.X, shoulderPos.Y, XWindow, YWindow);
                }
                if (i < joints.Length - 1)
                {
                    var Xnext = ConvertMathToWindow(joints[i + 1], shoulderPos).X;
                    var Ynext = ConvertMathToWindow(joints[i + 1], shoulderPos).Y;
                    graphics.DrawLine(ManipulatorPen, XWindow, YWindow, Xnext, Ynext);
                }
            }
        }

		private static void DrawReachableZone(
            Graphics graphics, 
            Brush reachableBrush, 
            Brush unreachableBrush, 
            PointF shoulderPos, 
            PointF[] joints)
		{
			var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
			var rmax = Manipulator.UpperArm + Manipulator.Forearm;
			var mathCenter = new PointF(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
			var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
			graphics.FillEllipse(reachableBrush, windowCenter.X - rmax, windowCenter.Y - rmax, 2 * rmax, 2 * rmax);
			graphics.FillEllipse(unreachableBrush, windowCenter.X - rmin, windowCenter.Y - rmin, 2 * rmin, 2 * rmin);
		}

		public static PointF GetShoulderPos(Form form)
		{
			return new PointF(form.ClientSize.Width / 2f, form.ClientSize.Height / 2f);
		}

		public static PointF ConvertMathToWindow(PointF mathPoint, PointF shoulderPos)
		{
			return new PointF(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
		}

		public static PointF ConvertWindowToMath(PointF windowPoint, PointF shoulderPos)
		{
			return new PointF(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
		}
	}
}
using System;

namespace Rectangles
{
	public static class RectanglesTask
	{
		// Пересекаются ли два прямоугольника (пересечение только по границе также считается пересечением)
		public static bool AreIntersected(Rectangle r1, Rectangle r2)
		{
			// так можно обратиться к координатам левого верхнего угла первого прямоугольника: r1.Left, r1.Top
			if ((r1.Top < r2.Bottom || r2.Top < r1.Bottom) && (r1.Right < r2.Left || r2.Right < r1.Left)) return false;
			else if ((r1.Bottom < r2.Top || r2.Bottom < r1.Top) && (r1.Left < r2.Right || r2.Left < r1.Right)) return false;

			else return true;
        }

		// Площадь пересечения прямоугольников
		public static int IntersectionSquare(Rectangle r1, Rectangle r2)
		{
			if (!AreIntersected(r1, r2)) return 0;
			int topMax = Math.Max(r1.Top, r2.Top);
			int bottomMin = Math.Min(r1.Bottom, r2.Bottom);
			int leftMax = Math.Max(r1.Left, r2.Left);
			int rightMin = Math.Min(r1.Right, r2.Right);

			int area = (bottomMin - topMax) * (rightMin - leftMax);

			return area;
		}

		// Если один из прямоугольников целиком находится внутри другого — вернуть номер (с нуля) внутреннего.
		// Иначе вернуть -1
		// Если прямоугольники совпадают, можно вернуть номер любого из них.
		public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
		{
			
			if (AreIntersected(r1,r2))
			{
				int area1 = r1.Width * r1.Height;
				int area2 = r2.Width * r2.Height;

				//if (IntersectionSquare(r1, r2) == area1) return 0;
				//else if (IntersectionSquare(r1, r2) == area2) return 1;
				//else return -1;

				if (r1.Top >= r2.Top && r1.Bottom <= r2.Bottom && r1.Left >= r2.Left && r1.Right <= r2.Right) return 0;
				if (r1.Top <= r2.Top && r1.Bottom >= r2.Bottom && r1.Left <= r2.Left && r1.Right >= r2.Right) return 1;
				else return -1;
			}
			else return -1;
		}
	}
}
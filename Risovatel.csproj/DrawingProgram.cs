using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace RefactorMe
{
    class Painter
    {
        static float x, y;
        static Graphics graphic;

        public static void Initialize ( Graphics newGraphic )
        {
            graphic = newGraphic;
            graphic.SmoothingMode = SmoothingMode.None;
            graphic.Clear(Color.Black);
        }

        public static void SetPosition(float x0, float y0)
        {x = x0; y = y0;}

        public static void MakeIt(Pen pen, double length, double angle)
        {
            //Делает шаг длиной dlina в направлении ugol и рисует пройденную траекторию
            var x1 = (float)(x + length * Math.Cos(angle));
            var y1 = (float)(y + length * Math.Sin(angle));
            graphic.DrawLine(pen, x, y, x1, y1);
            x = x1;
            y = y1;
        }

        public static void Change(double length, double angle)
        {
           x = (float)(x + length * Math.Cos(angle)); 
           y = (float)(y + length * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        public static void Draw(int width, int height, double rotationAngle, Graphics graphic)
        {
            // ugolPovorota пока не используется, но будет использоваться в будущем
            Painter.Initialize(graphic);
            float coefSmall = 0.04f;
            float coefBig = 0.375f;
            var sz = Math.Min(width, height);

            var diagonal_length = Math.Sqrt(2) * (sz * coefBig + sz * coefSmall) / 2;
            var x0 = (float)(diagonal_length * Math.Cos(Math.PI / 4 + Math.PI)) + width / 2f;
            var y0 = (float)(diagonal_length * Math.Sin(Math.PI / 4 + Math.PI)) + height / 2f;

            Painter.SetPosition(x0, y0);

            //Рисуем 1-ую сторону
            for (int i = 0; i < 4; i++)
            {
                DrawLine(coefSmall, coefBig, sz, ( - Math.PI / 2) * i); 
            }
        }

        private static void DrawLine(float coefSmall, float coefBig, int sz, double angle)
        {
            Painter.MakeIt(Pens.Yellow, sz * coefBig, 0 + angle);
            Painter.MakeIt(Pens.Yellow, sz * coefSmall * Math.Sqrt(2), Math.PI / 4 + angle);
            Painter.MakeIt(Pens.Yellow, sz * coefBig, Math.PI + angle);
            Painter.MakeIt(Pens.Yellow, sz * coefBig - sz * coefSmall, Math.PI / 2 + angle);
            
            Painter.Change(sz * coefSmall, -Math.PI+angle);
            Painter.Change(sz * coefSmall * Math.Sqrt(2), 3 * Math.PI / 4+angle);
        }
    }
}
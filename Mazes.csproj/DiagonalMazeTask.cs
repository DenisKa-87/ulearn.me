using System;

namespace Mazes
{
	public static class DiagonalMazeTask
	{
        public static void MoveOut(Robot robot, int width, int height)
        { 
            if (width >= height)
            {
                while (true)
                {
                    RobotMoveRight(robot, LongStep(width, height));
                    if (robot.Finished) break;
                    RobotMoveDown(robot, 1);
                }
            }
            else
            {
                while (true)
                {
                    RobotMoveDown(robot, LongStep(width, height));
                    if (robot.Finished) break;
                    RobotMoveRight(robot, 1);
                }
            }

        }

        public static int LongStep(int width, int height)
        {
            int longStep = (Math.Max(width, height)-3)%(Math.Min(width,height) -3);
            return longStep;
        }

        public static void RobotMoveRight(Robot robot, int stepLength)
        {
			for (int i = 0; i < stepLength; i++)
                    robot.MoveTo(Direction.Right);
        }

        public static void RobotMoveDown(Robot robot, int stepLength)
        {
            for (int i = 0; i < stepLength; i++)
                robot.MoveTo(Direction.Down);
        }
    }
}
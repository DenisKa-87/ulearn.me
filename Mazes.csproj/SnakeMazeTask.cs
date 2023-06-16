namespace Mazes
{
    public static class SnakeMazeTask
    {
        public static void MoveOut(Robot robot, int width, int height)
        {
            while (!robot.Finished)
            {
                RobotMoveRight(robot, width);
                RobotMoveDown(robot);
                RobotMoveLeft(robot);
                if (!robot.Finished)
                    RobotMoveDown(robot);
            }
        }

        public static void RobotMoveRight(Robot robot, int width)
        {
            while (robot.X < width - 2)
                robot.MoveTo(Direction.Right);
        }

        public static void RobotMoveDown(Robot robot)
        {
            for (int i = 0; i < 2; i++) robot.MoveTo(Direction.Down);
        }

        public static void RobotMoveLeft(Robot robot)
        {
            while (robot.X > 1)
                robot.MoveTo(Direction.Left);
        }

    }
}
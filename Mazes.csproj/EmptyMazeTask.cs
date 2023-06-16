namespace Mazes
{
	public static class EmptyMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
			robotMoveRight(robot, width);
			robotMoveDown(robot, height);
		}

		public static void robotMoveRight(Robot robot, int width)
		{
			while (robot.X < width-2 ) robot.MoveTo(Direction.Right);
        }

		public static void robotMoveDown(Robot robot, int height)
		{
            while (robot.Y < height -2) robot.MoveTo(Direction.Down);
        }

	}
}
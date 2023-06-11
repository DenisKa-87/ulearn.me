using Avalonia;
using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Dungeon;

public class DungeonTask
{
    public static MoveDirection[] FindShortestPath(Map map)
    {
        var startToChest = BfsTask.FindPaths(map, map.InitialPosition, map.Chests).ToArray();
        var exitToChest = BfsTask.FindPaths(map, map.Exit, map.Chests).ToArray();
        var paths = from toChest in startToChest
                    join fromChest in exitToChest
                    on toChest.Value equals fromChest.Value
                    select new { toChest, fromChest };
        var path = GetPath(paths, map);
        if (path == null)
            return new MoveDirection[0];
        return GetDirections(path);
    }

    public static Point[] GetPoints(SinglyLinkedList<Point> startToChest, SinglyLinkedList<Point> chestToFinish)
    {
        var path1 = new Point[startToChest.Length];
        var path2 = new Point[chestToFinish.Length];
        for (int i = 0; i < path1.Length; i++)
        {
            path1[i] = startToChest.Value;
            startToChest = startToChest.Previous;
        }
        Array.Reverse(path1);
        for (int i = 0; i < path2.Length; i++)
        {
            path2[i] = chestToFinish.Value;
            chestToFinish = chestToFinish.Previous;
        }
        var result = path1.Take(path1.Length - 1).Concat(path2).ToArray();
        return result;
    }

    public static Point[] GetPoints(SinglyLinkedList<Point> path)
    {
        var result = new Point[path.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = path.Value;
            path = path.Previous;
        }
        Array.Reverse(result);
        return result;
    }

    public static MoveDirection[] GetDirections(Point[] points)
    {
        MoveDirection[] result = new MoveDirection[points.Length - 1];
        for (int i = 0; i < points.Length - 1; i++)
        {
            result[i] = Walker.ConvertOffsetToDirection((points[i + 1] - points[i]));
        }
        return result;
    }

    public static Point[] GetPath(IEnumerable<dynamic> paths, Map map)
    {
        Point[] points;
        if (paths.Count() == 0)
        {
            var exit = new Point[] { map.Exit };
            var pathToExit = BfsTask.FindPaths(map, map.InitialPosition, exit).Where(x => x != null).Take(1).ToArray();
            if (pathToExit.Length == 0)
                return null;
            points = GetPoints(pathToExit[0]);
        }
        else
        {
            var shortestPath = paths.OrderBy(x => x.toChest.Length - 1 + x.fromChest.Length).First();
            points = GetPoints(shortestPath.toChest, shortestPath.fromChest);
        }
        return points;
    }
}
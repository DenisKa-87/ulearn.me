using System;
using System.Collections.Generic;

namespace Rivals;

public class RivalsTask
{
    public static IEnumerable<OwnedLocation> AssignOwners(Map map)
    {
        return GetOwnedLocations(map);
    }

    public static IEnumerable<OwnedLocation> GetOwnedLocations(Map map)
    {
        var queue = InitializeQueue(map);
        var marked = new HashSet<Point>();

        while (queue.Count > 0)
        {
            var playerMove = queue.Dequeue();
            if (marked.Contains(playerMove.Location))
                continue;
            foreach (var direction in GetPossibleDirections())
            {
                var nextLocation = playerMove.Location + new Point(direction.Item1, direction.Item2);
                if (map.InBounds(nextLocation) && map.Maze[nextLocation.X, nextLocation.Y] != MapCell.Wall
                    && !marked.Contains(nextLocation))
                {
                    var playerNextMove = new OwnedLocation(playerMove.Owner, nextLocation, playerMove.Distance +
                        Math.Abs(direction.Item1 + direction.Item2));
                    queue.Enqueue(playerNextMove);
                }
            }
            yield return playerMove;
            marked.Add(playerMove.Location);
        }
    }

    public static IEnumerable<Tuple<int, int>> GetPossibleDirections()
    {
        for (int x = -1; x < 2; x++)
        {
            for (int z = -1; z < 2; z++)
            {
                if (x != 0 && z != 0)
                    continue;
                yield return Tuple.Create(x, z);
            }
        }
    }

    public static Queue<OwnedLocation> InitializeQueue(Map map)
    {
        var queue = new Queue<OwnedLocation>();
        int playerId = 0;
        foreach (var player in map.Players)
        {
            queue.Enqueue(new OwnedLocation(playerId, player, 0)); ;
            playerId++;
        }
        return queue;
    }
}
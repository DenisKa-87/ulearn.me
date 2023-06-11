using Avalonia.FreeDesktop.DBusIme;
using DynamicData;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
	public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
	{
		var queue = new Queue<SinglyLinkedList<Point>>();
        var path = new SinglyLinkedList<Point>(start);
        queue.Enqueue(path);
        var visited = new HashSet<Point>();
        var nextSteps = Walker.PossibleDirections;
        var chestsStatus = chests.ToDictionary(chest => chest, chest => false);
        while (queue.Count > 0)
		{
            var currentStep = queue.Dequeue();
            if (visited.Contains(currentStep.Value))
                continue;
            visited.Add(currentStep.Value);

            foreach (var step in nextSteps )
            {
                var nextStep = new SinglyLinkedList<Point> ((currentStep.Value + step), currentStep);
                if (!map.InBounds(nextStep.Value))
                    continue; 
                if (chestsStatus.ContainsKey(nextStep.Value) && !chestsStatus[nextStep.Value])
                {
                    yield return nextStep;
                    chestsStatus[nextStep.Value] = true;
                }
                if (map.Dungeon[nextStep.Value.X, nextStep.Value.Y] != MapCell.Wall)
                    queue.Enqueue(nextStep);
            }           
        }
	}
}
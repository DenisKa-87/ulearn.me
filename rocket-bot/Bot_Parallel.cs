using Avalonia.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
	public Rocket GetNextMove(Rocket rocket)
	{
		// TODO: распараллелить запуск SearchBestMove
		//var threads = new Task<Tuple<Turn, double>>[threadsCount];
        var threads = new Task<(Turn, double)>[threadsCount];

        for (var i = 0; i < threadsCount; i++)
        {
            var rnd = new Random(i);
			threads[i] = Task.Run(() =>
                SearchBestMove(rocket, new Random(rnd.Next()), iterationsCount / threadsCount));
        }
		while (true)
		{
            if (threads.All(x => x.IsCompleted))
            {
                return rocket.Move(threads.OrderByDescending(x => x.Result.Item2).First().Result.Item1, level);
            }
        }	
	}
	
	public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
	{
		return new() { Task.Run(() => SearchBestMove(rocket, new Random(random.Next()), iterationsCount)) };
	}
}
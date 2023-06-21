using NUnit.Framework;
using NUnitLite;
using System;
using System.Collections.Generic;

namespace BinaryTrees;

class Program
{
	static void Main(string[] args)
	{
		//new AutoRun().Execute(args);
		var arr = new[] { 7, 6, 0, 5, 2, 4, 8, 3, 1, 9 };
		var tree = new BinaryTree<int>();
		foreach(var item in arr)
		{
			tree.Add(item);
			Console.Write(item + " ");
		}
		Console.WriteLine();
		Console.WriteLine("Adding has been completed!");
		//Console.WriteLine(tree.Right.Right.GetIndex());
		PrintTreeWithIndexes(tree);

    }

	public static void PrintTreeWithIndexes(BinaryTree<int> tree)
	{
		var current = tree.Left;
		while(current != null)
		{
            Console.WriteLine($"index[{current.GetIndex()}] = {current.Value}");
			current = current.Left;
        }
		Console.WriteLine($"index[{tree.GetIndex()}] = {tree.Value}");
		current = tree.Right;
        while (current != null)
        {
            Console.WriteLine($"index[{current.GetIndex()}] = {current.Value}");
            current = current.Right;
        }

    }
}


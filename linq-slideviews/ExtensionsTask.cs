using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
	/// <summary>
	/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
	/// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
	/// </summary>
	/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
	public static double Median(this IEnumerable<double> items)
	{
		var arr = items.ToList();
		arr.Sort();
		var arrLen = arr.Count();

		if (arrLen == 0) throw new System.InvalidOperationException();
		return 
			arrLen % 2 != 0 ? arr[arrLen/2] :
			(arr[arrLen/2 - 1] + arr[arrLen/2])/2;
	}

	/// <returns>
	/// Возвращает последовательность, состоящую из пар соседних элементов.
	/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
	/// </returns>
	public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
	{
		var en = items.GetEnumerator();
		var cont = en.MoveNext();
		
		while (cont)
		{
			var a = en.Current;
			cont = en.MoveNext();
			if (!cont) break;
			var b = en.Current;
			yield return (a, b);
		}
		
	}
}
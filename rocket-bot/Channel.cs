using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace rocket_bot;

public class Channel<T> where T : class
{
	/// <summary>
	/// Возвращает элемент по индексу или null, если такого элемента нет.
	/// При присвоении удаляет все элементы после.
	/// Если индекс в точности равен размеру коллекции, работает как Append.
	/// </summary>
	/// 

	private readonly List<T> lst = new List<T>();
	public T this[int index]
	{
		get
		{
			lock (lst) 
			{
				try
				{
					return lst[index];
				}
				catch
				{
					return null;
				}
			};
		}
		set
		{
			lock (lst)
			{
				if (index == lst.Count)
				{
					lst.Add(value);
				}
				else
				{
					lst[index] = value;
					var lstCount = lst.Count;
                    for (int i = index+1; i < lstCount; i++)
					{
						lst.RemoveAt(lst.Count - 1);
					}

                }
				
			}
		}
	}

	/// <summary>
	/// Возвращает последний элемент или null, если такого элемента нет
	/// </summary>
	public T LastItem()
	{
		lock (lst)
		{
			return lst.LastOrDefault();
		};
	}

	/// <summary>
	/// Добавляет item в конец только если lastItem является последним элементом
	/// </summary>
	public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
	{
		lock(lst)
		{
			if (knownLastItem == LastItem())
			{
				lst.Add(item);
			}

		};
	}

	/// <summary>
	/// Возвращает количество элементов в коллекции
	/// </summary>
	public int Count
	{
		get
		{
			lock (lst)
			{
				
				return lst.Count;
				
			};
		}
	}
}
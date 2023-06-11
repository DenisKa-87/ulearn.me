using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        /// <returns>
        /// Возвращает первую фразу словаря, начинающуюся с prefix.
        /// </returns>
        /// <remarks>
        /// Эта функция уже реализована, она заработает, 
        /// как только вы выполните задачу в файле LeftBorderTask
        /// </remarks>
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];

            return null;
        }

        /// <returns>
        /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
        /// элементов словаря, начинающихся с prefix.
        /// </returns>
        /// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            int start = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            int wordCount = GetCountByPrefix(phrases, prefix);
            if (wordCount <= 0) return new string[0];
            if (wordCount < count) count = wordCount;
            var result = new string[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = phrases[i + start];
            }
            // тут стоит использовать написанный ранее класс LeftBorderTask
            return result;
        }

        /// <returns>
        /// Возвращает количество фраз, начинающихся с заданного префикса
        /// </returns>
        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            int leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            int rightBorder = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
            if (rightBorder > leftBorder)
                return rightBorder - leftBorder - 1;
            // тут стоит использовать написанные ранее классы LeftBorderTask и RightBorderTask
            return -1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            // ...
            //CollectionAssert.IsEmpty(actualTopWords);
        }

        // ...

        [Test]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
        {
            // ...
            //Assert.AreEqual(expectedCount, actualCount);
        }

        // ...
    }
}

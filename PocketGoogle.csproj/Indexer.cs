using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private char[] splitters = new char[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
        private Dictionary<string, Dictionary<int, HashSet<int>>> wordsRegistry =
            new Dictionary <string, Dictionary<int, HashSet<int>>> ();
        public void Add(int id, string documentText)
        {
            var splittedText = documentText.Split(splitters);
            int currentPosition = 0;
            foreach (var word in splittedText)
            {
                AddWordToRegistry(word, id, currentPosition);
                currentPosition += word.Length + 1;
            }
        }

        private void AddWordToRegistry (string word, int id, int currentPosition)
        {
            if (!wordsRegistry.ContainsKey(word))
            {
                var positions = new HashSet<int>() { currentPosition };
                wordsRegistry.Add(word, new Dictionary<int, HashSet<int>>());
                wordsRegistry[word].Add(id, positions);
            }
            else if (!wordsRegistry[word].ContainsKey(id))
            {
                var positions = new HashSet<int>() { currentPosition };
                wordsRegistry[word].Add(id, positions);
            }
            else
            {
                wordsRegistry[word][id].Add(currentPosition);
            }
        }

        public List<int> GetIds(string word)
        {
            if (wordsRegistry.ContainsKey(word))
            {
                return new List <int>(wordsRegistry[word].Keys);
            }
            return new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
            if (wordsRegistry.ContainsKey(word))
                if (wordsRegistry[word].ContainsKey(id))
                    return new List<int>( wordsRegistry[word][id]);
            return new List<int>();
        }

        public void Remove(int id)
        {
            foreach (var word in wordsRegistry.Keys)
            {
                if (wordsRegistry[word].ContainsKey(id))
                {
                    wordsRegistry[word].Remove(id);
                }
            }
        }

        public Indexer()
        {
            wordsRegistry = new Dictionary<string, Dictionary<int, HashSet<int>>>();
        }
    }
}

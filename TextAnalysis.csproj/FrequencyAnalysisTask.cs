using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            //var result = new Dictionary<string, string>();
            SortedDictionary<Tuple<string, string>,int> nGrams = new SortedDictionary<Tuple<string, string>, int>();
            AddToNGramFrequencyDict(nGrams, text);
            Console.WriteLine("NGrams are created {0}", nGrams.Count);
            var result = nGrams.GroupBy(x => x.Key.Item1).Select(x => x.OrderByDescending(y => y.Value)
            .ThenBy(z => z.Key.Item2, StringComparer.Ordinal).First()).ToDictionary(x=>x.Key.Item1, x =>x.Key.Item2);
            //Console.WriteLine("NGrams are created {0}", nGrams.Count);
            //File.WriteAllLines("ngrams.txt", nGrams.Select(x =>  x.Key + " " + x.Value).ToArray());
            //AddToResult(nGrams, result);
            //File.WriteAllLines("result.txt", result.Select(x => "[" + x.Key + " " + x.Value + "]").ToArray());
            //foreach(var x in result) { Console.WriteLine(x); }
            return result;
        }
        private static void AddToResult(SortedDictionary<Tuple<string,string>, int> nGrams, Dictionary<string, string> result)
        {
            if (nGrams.Count == 0) { return; }
            if (nGrams.Count == 1) InsertInResult(nGrams.ElementAt(0));
            else
            {
                for (int i = 0; i <= nGrams.Count-1; i++)
                {
                    Console.WriteLine(i);
                    if (i == nGrams.Count-1)
                    {
                        InsertInResult(nGrams.ElementAt(i));
                        break;
                    }
                
                    for (int b = i+1; b < nGrams.Count; b++)
                    {

                        //Console.WriteLine("b = {0}", b);
                        if (nGrams.ElementAt(i).Key.Item1 == nGrams.ElementAt(b).Key.Item1
                            && nGrams.ElementAt(i).Value < nGrams.ElementAt(b).Value)
                        {
                            //Console.WriteLine(" first cond i = {0}", i);
                            //Console.WriteLine(nGrams.ElementAt(i).Key.Item1);
                            i = b;
                            //Console.WriteLine("now i = {0}", b);

                        }
                        else if (nGrams.ElementAt(i).Key.Item1 == nGrams.ElementAt(b).Key.Item1
                            && nGrams.ElementAt(i).Value < nGrams.ElementAt(b).Value
                            && string.CompareOrdinal(nGrams.ElementAt(i).Key.Item2, nGrams.ElementAt(b).Key.Item2) > 0)
                        {
                            //Console.WriteLine("second cond now i = {0}", i);
                            i = b;
                            //Console.WriteLine("now i = {0}", b);
                        }
                        else if (nGrams.ElementAt(i).Key.Item1 != nGrams.ElementAt(b).Key.Item1) break;
                    }
                    InsertInResult(nGrams.ElementAt(i));
                }
            }

            void InsertInResult (KeyValuePair<Tuple<string,string>,int> element)
            { 
                if (!result.ContainsKey(element.Key.Item1)) result.Add(element.Key.Item1, element.Key.Item2);  
            }
        }

        public static void AddToNGramFrequencyDict(SortedDictionary<Tuple<string, string>, int> nGrams, List<List<string>> text)
        {
             
            
            foreach (var sentence in text)
            {
                int count = sentence.Count;
                if (count < 2) continue;
                else if (count == 2)
                {
                    Tuple<string,string> nGram = GetNGram(sentence[0], sentence[1]);
                    AddToNGrams(nGram, nGrams);
                }
                else if (count > 2)
                {
                    for (int i = 0; i < count - 1; i++)
                    {
                        var nGram = GetNGram(sentence[i], sentence[i + 1]);
                        AddToNGrams(nGram, nGrams);
                    }
                    for (int i = 1; i < count - 1; i++)
                    {
                        var nGram = GetNGram(sentence[i - 1], sentence[i], sentence[i + 1]);
                        AddToNGrams(nGram, nGrams);
                    }
                }
            }
        }

        private static Tuple<string, string> GetNGram(string word0, string word1)
        {
            return new Tuple<string, string>(word0, word1);
        }
        private static Tuple<string, string> GetNGram(string word0, string word1, string word2)
        {
            return new Tuple<string, string>(word0 + " " + word1, word2);
        }
        private static void AddToNGrams(Tuple<string,string> nGram, SortedDictionary<Tuple<string, string>, int> nGrams)
        {   
            if (nGrams.ContainsKey(nGram)) nGrams[nGram]++;
            else nGrams.Add(nGram, 1);
        }
    }
}


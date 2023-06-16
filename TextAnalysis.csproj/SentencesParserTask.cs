using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            char[] separators = new char[] { '.', '!', '?', ';', ':', '(', ')' };
            foreach (string s in text.Split(separators))
            {
                if (GetWords(s).Count > 0) sentencesList.Add(GetWords(s));
            }
            /*foreach (var e in sentencesList)
                foreach(var t in e) Console.WriteLine(t);*/
            return sentencesList;
        }

        public static List<string> GetWords(string sentence)
        {
            var words = new List<string>();
            List<char> separators = new List<char>();
            foreach (char c in sentence)
            {
                if (!char.IsLetter(c) && c != '\'')
                {
                    separators.Add(c);
                }
            }
            char[] separatorArray = separators.Distinct().ToArray();
            foreach (var word in sentence.Split(separatorArray))
            {
                if (IsWord(word)) words.Add(word.ToLower());
            }
            words.RemoveAll(s => string.IsNullOrEmpty(s));
            return words;
        }

        public static bool IsWord(string word)
        {
            foreach (char c in word)
            {
                if (!char.IsLetter(c) && c != '\'') return false;
            }
            return true;
        }
    }
}
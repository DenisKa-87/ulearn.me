using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            var wordsList = phraseBeginning.Split().ToList();
            for (int i = 0; i < wordsCount; i++)
            {
                int phraseLength = wordsList.Count;
                if (wordsList.Count == 0) break;
                if (wordsList.Count == 1)
                {
                    if (nextWords.ContainsKey(wordsList[0]))
                    {
                        wordsList.Add(nextWords[wordsList[0]]);
                    }
                    else break;
                }
                else if (wordsList.Count >= 2)
                {
                    if (nextWords.ContainsKey(wordsList[phraseLength - 2] + " "
                        + wordsList[phraseLength - 1]))
                    {
                        wordsList.Add(nextWords[wordsList[phraseLength - 2] + " "
                            + wordsList[phraseLength - 1]]);
                    }
                    else if (!nextWords.ContainsKey(wordsList[phraseLength - 2] + " "
                        + wordsList[phraseLength - 1]) && nextWords.ContainsKey(wordsList[phraseLength - 1]))
                    {
                        wordsList.Add(nextWords[wordsList[phraseLength - 1]]);
                    }
                    else break;
                }  
            }
            return string.Join(" ", wordsList); ;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }
        // Скопируйте сюда метод с тестами из предыдущей задачи.
        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("a \"b\"", new[] { "a", "b" })]
        [TestCase("'a' b", new[] { "a", "b" })]
        [TestCase("a  \"b\"", new[] { "a", "b" })]
        [TestCase("'\"1\" \"2\" \"3\"'", new[] { "\"1\" \"2\" \"3\"" })]
        [TestCase(@"'a\' b", new[] { @"a' b" })]
        [TestCase("''", new[] { "" })]
        [TestCase("  a  ", new[] { "a" })]
        [TestCase("\"a 'b' 'c' d\"", new[] { "a \'b\' \'c\' d" })]
        [TestCase("a\"c\"", new[] { "a", "c" })]
        [TestCase(@"'\\'", new[] { @"\\" })]
        //[TestCase("\"\"\"\"", new[] { "\"" })]
        [TestCase(@"'ab ", new[] { "ab " })]
        [TestCase("", new string[] { })]

        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
        
    }

    public class FieldsParserTask
    {
        // При решении этой задаче постарайтесь избежать создания методов, длиннее 10 строк.
        // Подумайте как можно использовать ReadQuotedField и Token в этой задаче.
        public static List<Token> ParseLine(string line)
        {
            var tokenList = new List<Token>();
            int len = line.Length;
            int start = 0;
            char[] quotes = {'\'','"'};
            if (line.Length == 0)
            {
                //tokenList.Add(new Token("", 0, 0));
                return tokenList;
            }
            else
            {
                while (start < len)
                {
                    if (!quotes.Contains(line[start]) && line[start] != ' ')
                    {
                        var tempT = ReadField(line, start);
                        tokenList.Add(tempT);
                        start = tempT.GetIndexNextToToken();
                    }
                    else if (line[start] == ' ')
                    {
                        start++;
                        continue;
                    }
                    
                    else if (quotes.Contains(line[start]))
                    {
                        var tempT = ReadQuotedField(line, start);
                        tokenList.Add(tempT);
                        start = tempT.GetIndexNextToToken(); 
                    }
                }
            }


            return tokenList; // сокращенный синтаксис для инициализации коллекции.
        }
        
        private static Token ReadField(string line, int startIndex)
        {
            int len = line.Length;
            var sb = new StringBuilder();
            int expLen = 0;
            for (int i = startIndex; i < len; i++)
            {
                if (line[i] != ' ' && line[i] != '\'' && line[i] != '\"')
                {
                    sb.Append(line[i]);
                    expLen++;
                }
                else break;
            }
            return new Token(sb.ToString(), startIndex, expLen);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            
                return QuotedFieldTask.ReadQuotedField(line, startIndex);
            
        }
    }
}
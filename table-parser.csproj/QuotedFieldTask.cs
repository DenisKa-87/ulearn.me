
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("'a \"c\"'", 0, "a \"c\"", 7)]
        [TestCase("'\"1\" \"2\" \"3\"'",0, "\"1\" \"2\" \"3\"",13)]
        [TestCase("\"a 'b' 'c' d\"", 0, "a \'b\' \'c\' d", 13)]
        [TestCase("'def g h",0, "def g h", 8)]
       // [TestCase("'a' b",0, "a", 3)]
        [TestCase("'\\'", 0, "\\",3)]
        [TestCase("'\\\\'", 0, "\\\\", 4)]
        //[TestCase("'a' b",0,"a", 3)]
        //[TestCase("'a' b", 0, "a", 3)]
        [TestCase("'a' b'", 0, "a", 3)]
        [TestCase("'", 0, "", 1)]
        [TestCase(@"'a\' b", 0, @"a' b", 6)]
        [TestCase(@"'ab ", 0, "ab ", 4)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            int length = 1;
            var sb = new StringBuilder();
            if (line.Length == 1) return new Token("", startIndex, 1);
            for (int i = startIndex + 1; i < line.Length; i++)
            {
                if (line[i] == line[startIndex])
                {
                    length += 1;
                    break;
                }
                if (i < line.Length - 1)
                {
                    if (line[i] == '\\' && line[i + 1] == line[startIndex] && i < line.Length - 2)
                    {
                        length += 2;
                        i++;
                        sb.Append(line[i]);
                    }
                    else if (i < line.Length - 2 && line[i] == '\\' && line[i + 1] == line[startIndex])
                    {
                        length++;
                        sb.Append(line[i]);
                    }
                    
                    else
                    {
                        sb.Append(line[i]);
                        length++;
                    }
                }
                else if (i == line.Length - 1)
                {
                    sb.Append(line[i]);
                    length++;
                    break;
                }
            }
            return new Token(sb.ToString(), startIndex, length);
        }
    }
}

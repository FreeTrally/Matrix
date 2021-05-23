using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MatrixTest
{
    public class Console
    {
        public static Queue<string> TestData = new Queue<string>();
        public static void SetTestData(string testData)
        {
            TestData = new Queue<string>(testData.Split(new string[] { Environment.NewLine }, 
                StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimStart()));
        }
        public static string ReadLine()
        {
            return TestData.Dequeue();
        }
        public static void WriteLine(object value = null)
        {
            System.Console.WriteLine(value);
        }
        public static void Write(object value = null)
        {
            System.Console.WriteLine(value);
        }
    }

    class TestParsers
    {
        public static (List<int[]> matrix, bool correct) MatrixTestParser(string input)
        {
            Console.SetTestData(input);

            var s = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var len = s.Length;
            var matrix = new List<int[]>() { new int[len] };
            for (var i = 0; i < len; i++)
                matrix[0][i] = int.Parse(s[i]);

            var row = 1;

            while (s.Length > 0)
            {
                s = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (s.Length == 0)
                    break;

                if (s.Length != len)
                    return (matrix, false);

                matrix.Add(new int[len]);

                for (var j = 0; j < len; j++)
                    matrix[row][j] = int.Parse(s[j]);

                row++;
            }

            return (matrix, true);
        }

        public static (List<char> x, List<string> b, List<int[]> a, bool correct) SysTestParser(string input)
        {
            Console.SetTestData(input);

            var x = new List<char>();
            var b = new List<string>();
            var a = new List<int[]>();
            var k = new Dictionary<int, Dictionary<char, int>>();
      
            var i = 0;
            var s = Console.ReadLine().Split('=', StringSplitOptions.RemoveEmptyEntries);

            while (s.Length > 0)
            {
                if (s.Length != 2)
                    return (new List<char>(), new List<string>(), new List<int[]>(), false);

                b.Add(s[1]);
                k.Add(i, new Dictionary<char, int>());
                var termBuilder = new StringBuilder();

                for (var j = 0; j < s[0].Length + 1; j++)
                {
                    var ch = ' ';

                    if (j < s[0].Length)
                        ch = s[0][j];

                    if (termBuilder.Length > 0
                        && (ch == '+' || ch == '-' || j == s[0].Length))
                    {
                        var term = termBuilder.ToString();
                        var letter = term[term.Length - 1];
                        var numberStr = term.Substring(0, term.Length - 1);
                        var number = 1;

                        if (numberStr.Length > 1)
                            number = int.Parse(numberStr);
                        else if (numberStr.Length == 1 && numberStr[0] == '-')
                            number = -1;
                        else if (numberStr.Length == 1 && numberStr[0] != '+')
                            number = int.Parse(numberStr);

                        if (!x.Contains(letter))
                            x.Add(letter);

                        k[i].Add(letter, number);

                        termBuilder.Clear();
                    }

                    termBuilder.Append(ch);
                }

                s = Console.ReadLine().Split('=', StringSplitOptions.RemoveEmptyEntries);
                i++;
            }

            if (x.Count != k.Count)
                return (new List<char>(), new List<string>(), new List<int[]>(), false);

            foreach (var eqNum in k.Keys)
            {
                a.Add(new int[x.Count]);

                for (var j = 0; j < x.Count; j++)
                {
                    var letter = x[j];
                    if (k[eqNum].ContainsKey(letter))
                        a[eqNum][j] = k[eqNum][letter];
                    else
                        a[eqNum][j] = 0;
                }
            }

            return (x, b, a, true);
        }
    }
}

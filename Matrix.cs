using System;
using System.Text;
using System.Collections.Generic;

namespace Matrix
{
    class Matrix
    {
        public static (List<char>, List<string>, List<int[]>, bool) ParseSystem()
        {
            Console.WriteLine("Введите систему построчно без пробелов");
            Console.WriteLine("После ввода отправьте пустую строку");

            var x = new List<char>();
            var b = new List<string>();
            var a = new List<int[]>();
            var k = new Dictionary<int, Dictionary<char, int>>();
         
            var i = 0;
            var s = Console.ReadLine().Split('=', StringSplitOptions.RemoveEmptyEntries);

            while (s.Length > 0)
            {
                if (s.Length != 2)
                {
                    Console.WriteLine("Некорректное уравнение!");
                    return (new List<char>(), new List<string>(), new List<int[]>(), false);
                }

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
            {
                Console.WriteLine("Система не является квадратной!");
                return (new List<char>(), new List<string>(), new List<int[]>(), false);
            }

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

        public static (Dictionary<char, double>, bool) SolveSystem(List<char> x, 
            List<string> b, List<int[]> a)
        {
            var inverseResult = Inverse(a);

            if (!inverseResult.Item3)
            {
                Console.WriteLine("Систему однозначно решить не получилось");
                return (new Dictionary<char, double>(), false);
            }

            var aTWithB = VerMultiply(inverseResult.Item2, b.ToArray());

            if (!aTWithB.Item2)
            {
                Console.WriteLine("Систему однозначно решить не получилось");
                return (new Dictionary<char, double>(), false);
            }

            var solved = new Dictionary<char, double>();

            for (var i = 0; i < x.Count; i++)
            {
                solved[x[i]] = Math.Round(aTWithB.Item1[i, 0] / inverseResult.Item1, 3);
                if (Math.Abs(solved[x[i]]) == 0.0)
                    solved[x[i]] = 0.0;
            }

            return (solved, true);
        }

        public static void PrintSystem(Dictionary<char, double> solved)
        {
            Console.WriteLine("Результат решения системы");
            foreach (var letter in solved.Keys)
                Console.WriteLine(letter + " = " + solved[letter]);
        }

        public static (List<int[]>, bool) ParseMatrix()
        {
            Console.WriteLine("Вводите матрицу построчно, разделяя числа пробелом");
            Console.WriteLine("После ввода отправьте пустую строку");
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
                {
                    Console.WriteLine("Введена строка неверной длины!");
                    return (matrix, false);
                }

                matrix.Add(new int[len]);

                for (var j = 0; j < len; j++)
                    matrix[row][j] = int.Parse(s[j]);

                row++;
            }

            return (matrix, true);
        }

        public static int Determinant(List<int[]> mat, int m)
        {              
            if (m == 1)
                return mat[0][0];
            else if (m == 2)
                return mat[0][0] * mat[1][1] - (mat[1][0] * mat[0][1]);

            var k = 1;
            var d = 0;

            for (var i = 0; i < m; i++)
            {
                var p = GetMinorMatrix(mat, i, 0, m);
                d += k * mat[i][0] * Determinant(p, m - 1);
                k = -k;
            }

            return d;
        }

        private static List<int[]> GetMinorMatrix(List<int[]> mat, int i, int j, int m)
        {
            var p = new List<int[]>();
            var di = 0;

            for (var ki = 0; ki < m - 1; ki++)
            {
                if (ki == i)
                    di = 1;

                var dj = 0;
                p.Add(new int[m]);

                for (var kj = 0; kj < m - 1; kj++)
                {
                    if (kj == j)
                        dj = 1;
                    p[ki][kj] = mat[ki + di][kj + dj];
                }
            }

            return p;
        }

        public static (int[,], bool) VerMultiply(List<int[]> mat, string[] vector)
        {
            var result = new int[vector.Length, 1];

            for (var i = 0; i < mat.Count; i++)
            {
                result[i, 0] = 0;

                for (var j = 0; j < mat[i].Length; j++)
                    result[i, 0] += mat[i][j] * int.Parse(vector[j]);
            }

            return (result, true);
        }

        public static (int[,], bool) HorMultiply(List<int[]> mat, string[] vector)
        {
            var result = new int[mat.Count, vector.Length];

            for (var i = 0; i < mat.Count; i++)
            {
                for (var j = 0; j < vector.Length; j++)
                    result[i, j] = mat[i][0] * int.Parse(vector[j]);
            }

            return (result, true);
        }

        public static (int[,], bool) HandleMultiply(List<int[]> mat)
        {
            Console.WriteLine("Введите вектор-строку или вектор-столбец");
            Console.WriteLine("Вводите вектор-строку, разделяя числа пробелом");
            Console.WriteLine("Вводите вектор-столбец, отправляя по одному числу в строке");
            Console.WriteLine("После ввода вектора отправьте пустую строку");
            Console.WriteLine("Учтите, что вектор-строка может быть умножен " +
                "только на матрицу, являющуюся вектром-столбцом");

            var s = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var len = s.Length;

            if (len == 0)
                return (new int[0, 0], false);
            else if (len > 1)
            {
                if (mat.Count == len && mat[0].Length == 1)
                {
                    return HorMultiply(mat, s);
                }
                else
                {
                    Console.WriteLine("Введен вектор неверной длины!");
                    return (new int[0, 0], false);
                }
            }
            else if (len == 1 && mat.Count == 1 && mat[0].Length == 1)
            {
                return HorMultiply(mat, s);
            }

            var vector = new List<string>();
            vector.Add(s[0]);

            while (s.Length > 0)
            {
                s = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (s.Length == 0)
                    break;

                if (s.Length != 1)
                {
                    Console.WriteLine("Некорректный ввод вектора!");
                    break;
                }

                vector.Add(s[0]);
            }

            if (vector.Count == mat[0].Length)
                return VerMultiply(mat, vector.ToArray());
            else
                Console.WriteLine("Введен вектор неверной длины!");

            return (new int[0, 0], false);
        }

        public static void PrintMultiply(int[,] result)
        {
            Console.WriteLine("Результат умножения");

            for (var i = 0; i < result.GetLength(0); i++)
            {
                for (var j = 0; j < result.GetLength(1); j++)
                    Console.Write(result[i, j] + " ");
                Console.WriteLine();
            }
        }

        public static (double, List<int[]>, bool) Inverse(List<int[]> mat)
        {
            var alliedMat = new List<int[]>();

            if (mat.Count != mat[0].Length)
            {
                Console.WriteLine("Матрица не квадратная!");
                return (0.0, new List<int[]>(), false);
            }

            var det = Determinant(mat, mat.Count);
            if (det == 0)
            {
                Console.WriteLine("Определитель равен нулю!");
                return (0.0, new List<int[]>(), false);
            }

            for (var i = 0; i < mat.Count; i++)
            {
                alliedMat.Add(new int[mat.Count]);
                for (var j = 0; j < mat.Count; j++)
                {
                    var p = GetMinorMatrix(mat, i, j, mat.Count);
                    var minDet = Determinant(p, mat.Count - 1);
                    alliedMat[i][j] = (i + j) % 2 == 0 ? minDet : -minDet;
                }
            }

            var transposed = GetTransposed(alliedMat);

            return (det, transposed, true);
        }

        public static void PrintInverse(double det, List<int[]> transposed)
        {
            Console.WriteLine("Обратная матрица");

            for (var i = 0; i < transposed.Count; i++)
            {
                for (var j = 0; j < transposed.Count; j++)
                {
                    var divided = Math.Round(transposed[i][j] / det, 3);
                    if (Math.Abs(divided) == 0.0)
                        divided = 0.0;
                    Console.Write(divided + " ");
                }
                Console.WriteLine();
            }
        }

        public static List<int[]> GetTransposed(List<int[]> mat)
        {
            var result = new List<int[]>();
            var len = mat.Count;
            for (var i = 0; i < len; i++)
                result.Add(new int[len]);

            for (var i = 0; i < len; i++)
                for (var j = 0; j < len; j++)
                    result[j][i] = mat[i][j];

            return result;
        }
    }
}

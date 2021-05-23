using System;

namespace Matrix
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите номер команды (1-4)");
            Console.WriteLine("1 - Вычисление определителя");
            Console.WriteLine("2 - Вычисление обратной матрицы");
            Console.WriteLine("3 - Умножение матрицы на вектор");
            Console.WriteLine("4 - Решение системы квадратных уравнений");

            var s = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            while (s.Length > 0)
            {
                if (s.Length > 1)
                    Console.WriteLine("Неверный ввод команды!");

                switch (s[0])
                {
                    case "1":
                        DetCommand();
                        break;
                    case "2":
                        InvCommand();
                        break;
                    case "3":
                        MultCommand();
                        break;
                    case "4":
                        SysCommand();
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }

                Console.WriteLine("Введите номер команды (1-4)");
                s = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            }        
        }

        private static void DetCommand()
        {
            var matrix = Matrix.ParseMatrix();
            if (!matrix.Item2)
                return;

            if (matrix.Item1.Count != matrix.Item1[0].Length)
            {
                Console.WriteLine("Матрица не квадратная!");
                return;
            }

            var det = Matrix.Determinant(matrix.Item1, matrix.Item1.Count);
            Console.WriteLine("Определитель матрицы равен " + det);
        }

        private static void InvCommand()
        {
            var matrix = Matrix.ParseMatrix();
            if (!matrix.Item2)
                return;
            var inv = Matrix.Inverse(matrix.Item1);
            if (!inv.Item3)
                return;
            Matrix.PrintInverse(inv.Item1, inv.Item2);
        }

        private static void MultCommand()
        {
            var matrix = Matrix.ParseMatrix();
            if (!matrix.Item2)
                return;
            var mult = Matrix.HandleMultiply(matrix.Item1);
            if (!mult.Item2)
                return;
            Matrix.PrintMultiply(mult.Item1);
        }

        private static void SysCommand()
        {
            var parseResult = Matrix.ParseSystem();
            if (parseResult.Item4)
            {
                var systemResult = Matrix.SolveSystem(parseResult.Item1, 
                    parseResult.Item2, parseResult.Item3);

                if (systemResult.Item2)
                    Matrix.PrintSystem(systemResult.Item1);
            }
            else
                Console.WriteLine("Некорректный ввод!");
        }        
    }
}

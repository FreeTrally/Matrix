using NUnit.Framework;
using System;

namespace MatrixTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(@"
            2 2
            2 2
        ", 0)]
        [TestCase(@"
            -2 2
            2 2
        ", -8)]
        [TestCase(@"
            2
        ", 2)]
        [TestCase(@"
            2 2 3
            2 2 3
            0 0 0
        ", 0)]
        [TestCase(@"
            3 3 4 5
            1 2 3 4
            -3 5 -4 -5
            9 3 -3 4
        ", -400)]
        public void DeterminantWorksCorrectly(string input, int detExpected)
        {
            var matParse = TestParsers.MatrixTestParser(input);

            var det = Matrix.Matrix.Determinant(matParse.matrix, matParse.matrix.Count);

            Assert.AreEqual(det, detExpected);
        }

        [Test]
        [TestCase(@"
            3 1
            2 2 
            0 3
        ", new string[] { "-1", "1", "0", "2" },
           new string[] { "-3 3 0 6", "-2 2 0 4", "0 0 0 0", "-1 1 0 2" }, false)]
        [TestCase(@"
            3 2
            2 2 
            0 1
            -1 0
        ", new string[] { "-1", "1", "0"},
           new string[] { "-3 3 0 6", "-2 2 0 4", "0 0 0 0", "-1 1 0 2" }, false)]
        [TestCase(@"
            3
            2
        ", new string[] { "-1", "1"},
           new string[] { "-3 3", "-2 2"}, true)]
        [TestCase(@"
            3
            2
            0
            1
        ", new string[] {"-1", "1", "0", "2" }, 
           new string[] {"-3 3 0 6", "-2 2 0 4", "0 0 0 0", "-1 1 0 2" }, true)]
        public void HorMultiplyWorksCorrectly(string input, string[] vector,  
            string[] multExpected, bool isCorrect)
        {
            var matParse = TestParsers.MatrixTestParser(input);

            if (!matParse.correct || matParse.matrix[0].Length != 1)
            {
                Assert.AreEqual(false, isCorrect);
                return;
            }

            var mult = Matrix.Matrix.HorMultiply(matParse.matrix, vector);
            Assert.AreEqual(mult.Item2, isCorrect);
            var multMatrix = mult.Item1;

            for (var i = 0; i < multMatrix.GetLength(0); i++)
            {
                var expected = multExpected[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (var j = 0; j < multMatrix.GetLength(1); j++)
                    Assert.AreEqual(multMatrix[i, j], int.Parse(expected[j]));
            }
        }

        [Test]
        [TestCase(@"
            2 4 0
            -2 1 3
            -1 0 1
        ", new string[] { "1", "2", "-1" },
           new string[] { "10", "-3", "-2" }, true)]
        [TestCase(@"
            2 4
            -2 1
        ", new string[] { "1", "2", "-1" },
           new string[] { "10", "-3", "-2" }, false)]
        [TestCase(@"
            2 4 0
            -2 1 3
            -1 0 1
        ", new string[] { "1", "2" },
           new string[] { "10", "-3", "-2" }, false)]
        [TestCase(@"
            2 4 0
            -2 1 3
            -1 0 1
        ", new string[] { "0", "0", "0" },
           new string[] { "0", "0", "0" }, true)]
        public void VerMultiplyWorksCorrectly(string input, string[] vector, 
            string[] multExpected, bool isCorrect)
        {
            var matParse = TestParsers.MatrixTestParser(input);

            if (!matParse.correct || matParse.matrix[0].Length != vector.Length)
            {
                Assert.AreEqual(false, isCorrect);
                return;
            }

            var mult = Matrix.Matrix.VerMultiply(matParse.matrix, vector);
            Assert.AreEqual(mult.Item2, isCorrect);
            var multMatrix = mult.Item1;

            for (var i = 0; i < multMatrix.GetLength(0); i++)
            {
                var expected = multExpected[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (var j = 0; j < multMatrix.GetLength(1); j++)
                    Assert.AreEqual(multMatrix[i, j], int.Parse(expected[j]));
            }
        }


        [Test]
        [TestCase(@"
            2 3
            3 3
        ", -3, new string[] { "3 -3", "-3 2"}, true)]
        [TestCase(@"
            3 3
            3 3
        ", -3, new string[] { "3 -3", "-3 2" }, false)]
        [TestCase(@"
            2 3 3 
            3 3 3
        ", -3, new string[] { "3 -3", "-3 2" }, false)]
        [TestCase(@"
            2 3 3
            2 3 1
            4 2 2
        ", -16, new string[] { "4 0 -6", "0 -8 4", "-8 8 0" }, true)]
        public void InverseWorksCorrectly(string input, double detExpected, 
            string[] invExpected, bool isCorrect)
        {
            var matParse = TestParsers.MatrixTestParser(input);

            if (!matParse.correct || matParse.matrix.Count != matParse.matrix[0].Length)
            {
                Assert.AreEqual(false, isCorrect);
                return;
            }

            var inv = Matrix.Matrix.Inverse(matParse.matrix);

            if (!isCorrect)
            {
                Assert.AreEqual(inv.Item3, isCorrect);
                return;
            }
            
            Assert.AreEqual(inv.Item1, detExpected);

            var invMatrix = inv.Item2;

            for (var i = 0; i < invMatrix.Count; i++)
            {
                var expected = invExpected[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (var j = 0; j < invMatrix[i].Length; j++)
                    Assert.AreEqual(invMatrix[i][j], int.Parse(expected[j]));
            }
        }

        [Test]
        [TestCase(@"
            2x+3y+2z=9
            x+2y-3z=14
            3x+4y+z=16
        ", new char[] { 'x', 'y', 'z' }, new double[] { 2, 3, -2 }, true)]
        [TestCase(@"
            2x+3y+2z=9
            x+2y-3z=14
            3x+4y+z=16
            x+y=1
        ", new char[] { 'x', 'y', 'z' }, new double[] { 2, 3, -2 }, false)]
        [TestCase(@"
            2x+3y+2z=9
            x+2y-3z=14
        ", new char[] { 'x', 'y', 'z' }, new double[] { 2, 3, -2 }, false)]
        [TestCase(@"
            -x-2y=4
            -y+x=10
        ", new char[] { 'x', 'y' }, new double[] { 5.333, -4.667 }, true)]
        [TestCase(@"
            -x=0
        ", new char[] { 'x' }, new double[] { 0.0 }, true)]
        public void SolveSystemWorksCorrectly(string input, char[] sysLetters, 
            double[] sysVals, bool isCorrect)
        {
            var sysParse = TestParsers.SysTestParser(input);

            if (!sysParse.correct)
            {
                Assert.AreEqual(sysParse.correct, isCorrect);
                return;
            }

            var sys = Matrix.Matrix.SolveSystem(sysParse.x, sysParse.b, sysParse.a);

            Assert.AreEqual(sys.Item2, isCorrect);
            
            for (var i = 0; i < sysVals.Length; i++)
                Assert.AreEqual(sys.Item1[sysLetters[i]], sysVals[i]);
        }
    }
}

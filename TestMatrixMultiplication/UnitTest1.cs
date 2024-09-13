using System.Diagnostics;
using MatrixMultiplication;

namespace TestMatrixMultiplication
{
    public class Tests
    {
        [Test]
        public void TestMultiplicateMatrix()
        {
            Matrix matrix1 = new ("testMatrix1.txt");
            Matrix matrix2 = new ("testMatrix2.txt");
            var result = matrix1.MultiplicateMatrix(matrix2);
            result.WriteMatrixFile("testresult.txt");
            Assert.That(File.ReadAllText("testResult.txt"),
                Is.EqualTo(File.ReadAllText("testRightResult.txt")));
        }

        [Test]
        public void TestMultiThreadMultiplicateMatrix()
        {
            Matrix matrix1 = new("testMatrix1.txt");
            Matrix matrix2 = new("testMatrix2.txt");
            var result = matrix1.MultiThreadMultiplicateMatrix(matrix2);
            result.WriteMatrixFile("testresult.txt");
            Assert.That(File.ReadAllText("testResult.txt"),
                Is.EqualTo(File.ReadAllText("testRightResult.txt")));
        }

        [Test]
        public void TestInvalidMatrix()
        {
            Matrix matrix = default!;
            Assert.Throws<ArgumentException>(() => matrix = new("testInvalidMatrix.txt"));
        }

        [Test]
        public void TestMismatchMatrix()
        {
            Matrix matrix1 = new(3, 3);
            Matrix matrix2 = new(2, 3);
            Assert.Throws<MismatchRowsWithColumnsException>(() => matrix1.MultiplicateMatrix(matrix2));
        }

        [Test]
        public void TestMismatchMatrixMultiThread()
        {
            Matrix matrix1 = new(3, 3);
            Matrix matrix2 = new(2, 3);
            Assert.Throws<MismatchRowsWithColumnsException>(() => matrix1.MultiThreadMultiplicateMatrix(matrix2));
        }

        [Test]
        public void ComparisonMultiplicationModes()
        {
            Matrix matrix1 = new(1500, 1500);
            Matrix matrix2 = new(1500, 1500);
            matrix1.RandomizeMatrix();
            matrix2.RandomizeMatrix();

            Stopwatch stopwatch = new();

            stopwatch.Start();
            matrix1.MultiThreadMultiplicateMatrix(matrix2);
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            matrix1.MultiplicateMatrix(matrix2);
            stopwatch.Stop();
            Assert.IsTrue(time + (stopwatch.ElapsedMilliseconds * 0.3) < stopwatch.ElapsedMilliseconds);
        }
    }
}

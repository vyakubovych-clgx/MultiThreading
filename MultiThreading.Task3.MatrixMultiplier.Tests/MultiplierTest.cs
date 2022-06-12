using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        [DataRow(10, 10)]
        public void ParallelEfficiencyTest(int testCount, int successSequenceLength)
        {
            Console.WriteLine("Testing for the minimal matrix size where parallel multiplying takes less time then regular one.");
            Console.WriteLine($"Results from {testCount} different runs:");

            var results = new long[testCount];
            for (int i = 0; i < testCount; i++)
            {
                var size = 1L;
                var fasterInARow = 0;
                do
                {
                    var m1 = new Matrix(size, size, true);
                    var m2 = new Matrix(size, size, true);

                    var regularTimeSpent = GetMultiplyingTimeSpent(m1, m2, new MatricesMultiplier());
                    var parallelTimeSpent = GetMultiplyingTimeSpent(m1, m2, new MatricesMultiplierParallel());

                    if (regularTimeSpent > parallelTimeSpent)
                        fasterInARow++;
                    else
                        fasterInARow = 0;

                    size++;
                } while (fasterInARow < successSequenceLength);

                var result = size - successSequenceLength;
                Console.WriteLine(result);
                results[i] = result;
            }

            Console.WriteLine($"The minimal result is {results.Min()}");
            Console.WriteLine($"The average result is {results.Average()}");
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        TimeSpan GetMultiplyingTimeSpent(Matrix m1, Matrix m2, IMatricesMultiplier multiplier)
        {
            var stopwatch = Stopwatch.StartNew();
            multiplier.Multiply(m1, m2);
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        #endregion
    }
}

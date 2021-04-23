using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class Matrix
    {

        public static void Execute()
        {
            int columnSize = 500;
            int rowSize = 1000;
            int columnSize2 = 1500;

            int[,] a = CreateMatrix(columnSize, rowSize);
            int[,] b = CreateMatrix(columnSize2, columnSize);
            int[,] result = new int[rowSize, columnSize2];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Sequential(a, b, result);
            stopwatch.Stop();
            Console.WriteLine($"Sequential: {stopwatch.ElapsedMilliseconds} ms");


            stopwatch.Restart();
            MultiplyMatricesParallel(a, b, result);
            stopwatch.Stop();
            Console.WriteLine($"Parallel: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            MultiplyMatricesTasks(a, b, result);
            stopwatch.Stop();
            Console.WriteLine($"Tasks: {stopwatch.ElapsedMilliseconds} ms");


            stopwatch.Restart();
            MultiplyMatricesParallelPartitioner(a, b, result);
            stopwatch.Stop();
            Console.WriteLine($"ParallelPartitioner: {stopwatch.ElapsedMilliseconds} ms");

        }

        private static void Sequential(int[,] a, int[,] b, int[,] result)
        {
            int columnSizeA = a.GetLength(1), rowSizeA = a.GetLength(0), columnSizeB = b.GetLength(1);

            for (int i = 0; i < rowSizeA; i++)
            {
                for (int ii = 0; ii < columnSizeB; ii++)
                {
                    int temp = 0;
                    for (int iii = 0; iii < columnSizeA; iii++)
                    {
                        temp = a[i, iii] * b[iii, ii];
                    }
                    result[i, ii] += temp;
                }
            }
        }

        static void MultiplyMatricesTasks(int[,] a, int[,] b, int[,] result)
        {
            int columnSizeA = a.GetLength(1), rowSizeA = a.GetLength(0), columnSizeB = b.GetLength(1);


            var tasks = Enumerable.Range(0, rowSizeA).Select(i => Task.Run(() =>
            {
                //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                for (int j = 0; j < columnSizeB; j++)
                {
                    int temp = 0;
                    for (int k = 0; k < columnSizeA; k++)
                    {
                        temp += a[i, k] * b[k, j];
                    }
                    result[i, j] = temp;
                }
            }));

            Task.WaitAll(tasks.ToArray());
        }

        static void MultiplyMatricesParallel(int[,] a, int[,] b, int[,] result)
        {
            int columnSizeA = a.GetLength(1), rowSizeA = a.GetLength(0), columnSizeB = b.GetLength(1);

            Parallel.For(0, rowSizeA, i =>
            {
                //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                for (int j = 0; j < columnSizeB; j++)
                {
                    int temp = 0;
                    for (int k = 0; k < columnSizeA; k++)
                    {
                        temp += a[i, k] * b[k, j];
                    }
                    result[i, j] = temp;
                }
            });
        }

        static void MultiplyMatricesParallelPartitioner(int[,] a, int[,] b, int[,] result)
        {
            int columnSizeA = a.GetLength(1), rowSizeA = a.GetLength(0), columnSizeB = b.GetLength(1);

            var partitioner = Partitioner.Create(0, rowSizeA);


            Parallel.ForEach(partitioner, x =>
            {
                for (int i = x.Item1; i < x.Item2; i++)
                {
                    for (int ii = 0; ii < columnSizeB; ii++)
                    {
                        int temp = 0;
                        for (int iii = 0; iii < columnSizeA; iii++)
                        {
                            temp = a[i, iii] * b[iii, ii];
                        }
                        result[i, ii] += temp;
                    }
                }
            });
        }

        private static int[,] CreateMatrix(int columnSize, int rowSize)
        {
            var matrix = new int[rowSize, columnSize];
            Random r = new Random(columnSize + rowSize);
            for (int i = 0; i < rowSize; i++)
            {
                for (int ii = 0; ii < columnSize; ii++)
                {
                    matrix[i, ii] = r.Next(0, 100);
                }
            }

            return matrix;
        }
    }
}

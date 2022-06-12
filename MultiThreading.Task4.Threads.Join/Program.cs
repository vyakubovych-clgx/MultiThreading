/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        const int THREAD_COUNT = 10;
        static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(3);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();


            Console.WriteLine("Using Threads:");
            var thread = new Thread(CreateUsingThreads);
            thread.Start(THREAD_COUNT);
            thread.Join();

            Console.WriteLine("\nUsing ThreadPool:");
            ThreadPool.QueueUserWorkItem(CreateUsingThreadPool, THREAD_COUNT);

            Console.ReadLine();
        }

        static void CreateUsingThreads(object state)
        {
            DecrementActAndOutput(state, n =>
            {
                var thread = new Thread(CreateUsingThreads);
                thread.Start(n);
                thread.Join();
            });
        }

        static void CreateUsingThreadPool(object state)
        {
            DecrementActAndOutput(state, n =>
            {
                Semaphore.Wait();
                try
                {
                    ThreadPool.QueueUserWorkItem(CreateUsingThreadPool, n);
                }
                finally
                {
                    Semaphore.Release();
                }
            });
        }

        static void DecrementActAndOutput(object state, Action<int> action)
        {
            var number = (int) state - 1;
            if (number > 0)
                action(number);
            Console.WriteLine(number);
        }
    }
}

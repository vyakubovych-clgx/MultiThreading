/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static readonly List<int> Collection = new List<int>();
        static readonly object LockObject = new object();
        const int COLLECTION_SIZE = 10;
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var populateTask = Task.Run(PopulateCollection);
            var outputTask = Task.Run(() => OutputCollection(Collection));

            Task.WaitAll(populateTask, outputTask);
            Console.ReadLine();
        }

        static void PopulateCollection()
        {
            for (int i = 0; i < COLLECTION_SIZE; i++)
            {
                lock (LockObject)
                {
                    Collection.Add(i + 1);
                    Monitor.Pulse(LockObject);
                    if (i != COLLECTION_SIZE - 1)
                        Monitor.Wait(LockObject);
                }
            }
        }

        static void OutputCollection(List<int> collection)
        {
            for (int i = 0; i < COLLECTION_SIZE; i++)
            {
                lock (LockObject)
                {
                    foreach (var number in collection)
                        Console.Write($"{number} ");
                    Console.WriteLine();
                    Monitor.Pulse(LockObject);
                    if (i != COLLECTION_SIZE - 1)
                        Monitor.Wait(LockObject);
                }
            }
        }
    }
}

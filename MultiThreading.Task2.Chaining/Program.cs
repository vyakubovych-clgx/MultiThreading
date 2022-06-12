/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static readonly Random Random = new Random();
        private const int ARRAY_LENGTH = 10;
        private const int MAX_RANDOM_VALUE = 100;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Task.Run(CreateTenRandomIntegers)
                .ContinueWith(t => MuliplyByRandomInteger(t.Result))
                .ContinueWith(t => Sort(t.Result))
                .ContinueWith(t => Average(t.Result))
                .Wait();

            Console.ReadLine();
        }

        static int[] CreateTenRandomIntegers()
        {
            var result = Enumerable.Range(0, ARRAY_LENGTH).Select(n => GetRandomInt()).ToArray();
            OutputArray(result);
            return result;
        }
        static int[] MuliplyByRandomInteger(int[] array)
        {
            var multiplier = GetRandomInt();
            var result = array.Select(n => n * multiplier).ToArray();
            OutputArray(result);
            return result;
        }

        static int[] Sort(int[] array)
        {
            var result = array.OrderBy(n => n).ToArray();
            OutputArray(result);
            return result;
        }

        static double Average(int[] array)
        { 
            var result = array.Average();
            Console.WriteLine(result);
            return result;
        }

        static void OutputArray(int[] array)
        {
            foreach (var number in array)
                Console.Write($"{number} ");
            
            Console.WriteLine();
        }

        static int GetRandomInt() => Random.Next(MAX_RANDOM_VALUE);
    }
}

/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static CancellationTokenSource _source;
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            Console.WriteLine("a:");
            PerformTasks(TaskContinuationOptions.None);

            Console.WriteLine("\nb:");
            PerformTasks(TaskContinuationOptions.NotOnRanToCompletion);

            Console.WriteLine("\nc:");
            PerformTasks(TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            Console.WriteLine("\nd:");
            PerformTasks(TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

            Console.ReadLine();
        }

        static void PerformTasks(TaskContinuationOptions options)
        {
            _source = new CancellationTokenSource();

            var afterSuccess = Task.Run(SuccessTask).ContinueWith(SecondTask, options);
            var afterFail = Task.Run(FailTask).ContinueWith(SecondTask, options);
            var afterCancel = Task.Run(CancelledTask, _source.Token).ContinueWith(SecondTask, options);

            TryWaitAll(afterSuccess, afterFail, afterCancel);
        }

        static void SuccessTask()
        {
            Console.WriteLine($"First task status: RanToCompletion. Thread id: {Thread.CurrentThread.ManagedThreadId}");
        }

        static void FailTask()
        {
            Console.WriteLine($"First task status: Faulted. Thread id: {Thread.CurrentThread.ManagedThreadId}");
            throw new Exception();
        }

        static void CancelledTask()
        {
            Console.WriteLine($"First task status: Cancelled. Thread id: {Thread.CurrentThread.ManagedThreadId}");
            _source.Cancel();
            _source.Token.ThrowIfCancellationRequested();
        }

        static void SecondTask(Task previous)
        {
            Console.WriteLine(
                $"Previous task status: {previous.Status}. Thread id: {Thread.CurrentThread.ManagedThreadId}. IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
        }

        static void TryWaitAll(params Task[] tasks)
        {
            try
            {
                Task.WaitAll(tasks);
            }
            catch
            {

            }
        }

    }
}

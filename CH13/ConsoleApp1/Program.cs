using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using System.Net.Http;


namespace ConsoleApp1
{
    class Program
    {
        static Random rnd = new Random();
        static string msg;
        static object lockObj = new object();

        static async Task Main(string[] args)
        {
            await foreach(int num in GetNumbers())
            {
                WriteLine($"Number:{num}");
            }
        }

        async static IAsyncEnumerable<int> GetNumbers()
        {
            var rnd = new Random();
            await Task.Run(() => Task.Delay(rnd.Next(1500, 3000)));
            yield return rnd.Next(0, 1001);

            await Task.Run(() => Task.Delay(rnd.Next(1500, 3000)));
            yield return rnd.Next(0, 1001);

            await Task.Run(() => Task.Delay(rnd.Next(1500, 3000)));
            yield return rnd.Next(0, 1001);
        }

        static async void AsyncDemo()
        {
            using(HttpClient client=new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://www.apple.com/");
                WriteLine(response.Content.Headers.ContentLength);
            }
        }

        static void MonitorAvoidDeakLock()
        {
            try
            {
                if (Monitor.TryEnter(lockObj, TimeSpan.FromSeconds(15)))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Thread.Sleep(rnd.Next(200));
                        msg += "A";
                        Write(".");
                    }
                }
                else
                {
                    WriteLine("Method A failed to enter a monitor lock.");
                }
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
            
            
        }

        static void MultipleTaskWaitAll()
        {
            WriteLine("Please wait for the tasks to complete.");
            Stopwatch sw = Stopwatch.StartNew();
            Task a = Task.Factory.StartNew(MethodAA);
            Task b = Task.Factory.StartNew(MethodBB);
            Task.WaitAll(new Task[] { a, b });
            WriteLine();
            WriteLine($"Results:{msg}");
            WriteLine($"{sw.ElapsedMilliseconds:#,##0} elapsed milliseconds.");
        }

        static void MethodAA()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(rnd.Next(200));
                msg += "A";
                Write(".");
            }
        }

        static void MethodBB()
        { 
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(rnd.Next(200));
                msg += "B";
                Write(".");
            }
        }

        static void TaskWaitMethod()
        {
            var outer = Task.Factory.StartNew(OuterMethod);
            outer.Wait();
            WriteLine("Console app is stopping.");
        }

        static void OuterMethod()
        {
            WriteLine("Outer method starting...");
            var inner = Task.Factory.StartNew(InnerMethod, TaskCreationOptions.AttachedToParent);
            WriteLine("Outer method finished.");
        }

        static void InnerMethod()
        {
            WriteLine("Inner method starting...");
            Thread.Sleep(2000);
            WriteLine("Inner method finished.");
        }

        static void TaskContinueWith()
        {
            WriteLine("Passing the result of one task as an input into another.");
            var taskCallWebServiceAndThenStoredProcedure = Task.Factory.StartNew(CallWebService)
                .ContinueWith(x => CallStoredProcedure(x.Result));
            WriteLine($"Result:{taskCallWebServiceAndThenStoredProcedure.Result}");
        }

        static decimal CallWebService()
        {
            WriteLine("Starting call to web service...");
            Thread.Sleep((new Random()).Next(2000, 4000));
            WriteLine("Finished call to web service.");
            return 89.99M;
        }

        static string CallStoredProcedure(decimal amount)
        {
            WriteLine("Starting call to stored procedure...");
            Thread.Sleep((new Random()).Next(2000, 4000));
            WriteLine("Finished call to stored procedure.");
            return $"12 products cost more than {amount:C}";
        }

        static void TaskWaitAllDemo()
        {
            var timer = Stopwatch.StartNew();
            WriteLine("Running methods asynchronously on multiple threads.");
            Task taskA = new Task(MethodA);
            taskA.Start();
            Task taskB = Task.Factory.StartNew(MethodB);
            Task taskC = Task.Run(new Action(MethodC));
            Task[] tasks = { taskA, taskB, taskC };
            Task.WaitAll(tasks);
            timer.Stop();
            WriteLine($"{timer.ElapsedMilliseconds:#,##0} ms elapsed.");
        }

        static void AsynchronousDemo()
        {
            var timer = Stopwatch.StartNew();
            WriteLine("Running methods asynchronously on multiple threads.");
            Task taskA = new Task(MethodA);
            taskA.Start();
            Task taskB = Task.Factory.StartNew(MethodB);
            Task taskC = Task.Run(new Action(MethodC));
            timer.Stop();
            WriteLine($"{timer.ElapsedMilliseconds:#,##0} ms elapsed.");

        }

        static void SynchronousDemo()
        {
            var timer = Stopwatch.StartNew();
            WriteLine("Running methods synchronously on one thread.");
            MethodA();
            MethodB();
            MethodC();
            timer.Stop();
            WriteLine($"{timer.ElapsedMilliseconds:#,##0} ms elapsed.");
        }

        static void MethodA()
        {
            WriteLine("Starting Method A...");
            Thread.Sleep(3000);
            WriteLine("Finished Mehod A.");
        }

        static void MethodB()
        {
            WriteLine("Starting Method B...");
            Thread.Sleep(2000);
            WriteLine("Finished Method B.");
        }

        static void MethodC()
        {
            WriteLine("Starting Method C...");
            Thread.Sleep(1000);
            WriteLine("Finished Method C");
        }

        static void TestStringBuilder()
        {
            int[] numbers = Enumerable.Range(1, 50000).ToArray();
            WriteLine("Using string with+");
            Recorder.Start();
            string str = "";
            for(int i=0;i<numbers.Length;i++)
            {
                str += numbers[i] + ",";
            }
            Recorder.Stop();
            WriteLine("Using StringBuilder");
            Recorder.Start();

            var builder = new System.Text.StringBuilder();
            for(int i=0;i<numbers.Length;i++)
            {
                builder.Append(numbers[i]);
                builder.Append(",");
            }
            Recorder.Stop();
        }

        static void TestStopWatch()
        {
            WriteLine("Processing.Please wait...");
            Recorder.Start();
            int[] largeArrayOfInts = Enumerable.Range(1, 10000).ToArray();
            System.Threading.Thread.Sleep(new Random().Next(5, 10) * 1000);
            Recorder.Stop();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrent
{
    public static class CreateThreadsAndPassData
    {
        public static void CreateAThread() => Simple.MainThread();

        public static void PassDataToThreads() => Example1.MainTask();

        public static void RetriveDataFromThreads() => Example2.MainThread();
    }

    public class ServerClass
    {
        // The method that will be called when the thread is started.
        public void InstanceMethod()
        {
            Console.WriteLine("ServerClass.InstanceMethod is running on another thread.");

            // Pause for a moment to provide a delay to make threads more apparent.
            Thread.Sleep(3000);
            Console.WriteLine("The instance method called by the worker thread has ended.");
        }

        public static void StaticMethod()
        {
            Console.WriteLine("ServerClass.StaticMethod is running on another thread.");

            // Pause for a moment to provide a delay to make threads more apparent.
            Thread.Sleep(5000);
            Console.WriteLine("The static method called by the worker thread has ended.");
        }
    }

    public class Simple
    {
        public static void MainThread()
        {
            ServerClass serverObject = new ServerClass();

            // Create the thread object, passing in the serverObject.InstanceMethod method using a ThreadStart delegate.
            Thread InstanceCaller = new Thread(new ThreadStart(serverObject.InstanceMethod));

            // Start the thread.
            InstanceCaller.Start();

            Console.WriteLine("The MainThread() thread call this after starting the new InstanceCaller thread.");

            // Create the thread object, passing in the serverObject.StaticMethod method using a ThreadStart delegate.
            Thread StaticCaller = new Thread(new ThreadStart(ServerClass.StaticMethod));

            // Start the thread.
            StaticCaller.Start();

            Console.WriteLine("The MainThread() thread call this after starting the new StaticThread thread.");
        }
        // The example displays the output like the following:
        //    The Main() thread calls this after starting the new InstanceCaller thread.
        //    The Main() thread calls this after starting the new StaticCaller thread.
        //    ServerClass.StaticMethod is running on another thread.
        //    ServerClass.InstanceMethod is running on another thread.
        //    The instance method called by the worker thread has ended.
        //    The static method called by the worker thread has ended.
    }

    /// <summary>
    /// The ThreadWithState class contains the information needed for a task, and the method that executes the task.
    /// </summary>
    public class ThreadWithState
    {
        // State information used in the task.
        private string boilerplate;
        private int value;

        // The constructor obtains the state information.
        public ThreadWithState(string text, int number)
        {
            boilerplate = text;
            value = number;
        }

        // The thread procedure performs the task, such as formatting and printing a document.
        public void ThreadProc() => Console.WriteLine(boilerplate, value);
    }

    // Entry point for the example.
    public class Example1
    {
        public static void MainTask()
        {
            // Supply the state information required by the task.
            ThreadWithState tws = new ThreadWithState("This report displays the number {0}.", 42);

            // Create a thread to execute the task and then start the thread.
            Thread t = new Thread(new ThreadStart(tws.ThreadProc));
            t.Start();
            Console.WriteLine("Main thread does some work, then waits.");
            t.Join();
            Console.WriteLine("Independent task has completed, main thread ends.");
        }
        // The example displays the following output:
        //       Main thread does some work, then waits.
        //       This report displays the number 42.
        //       Independent task has completed; main thread ends.
    }


    // Delegate that defines the signature for the callback method.
    public delegate void ExampleCallback(int lineCount);

    /// <summary>
    /// The ThreadWithState2 class contains the information needed for a task, the method that executes the task,
    /// and a delegate to call when the task is complete.
    /// </summary>
    public class ThreadWithState2
    {
        // State information used in the task.
        private string boilerplate;
        private int value;

        // Delegate used to execute the callback method when the task is complete.
        private ExampleCallback callback;

        // The constructor obtains the state information and the callback delegate.
        public ThreadWithState2(string text, int number, ExampleCallback callbackDelegate)
        {
            boilerplate = text;
            value = number;
            callback = callbackDelegate;
        }

        // The thread procedure performs the task, such as formatting and printing a document, and then invokes the
        // callback delegate with the number of lines printed.
        public void ThreadProc()
        {
            Console.WriteLine(boilerplate, value);
            if (callback != null)
                callback(1);
        }
    }

    // Entry point for the example.
    public class Example2
    {
        public static void MainThread()
        {
            ThreadWithState2 tws = new ThreadWithState2("This report displays the number {0}.", 42, ResultCallback);

            Thread t = new Thread(new ThreadStart(tws.ThreadProc));
            t.Start();
            Console.WriteLine("Main thread does some work, then waits.");
            t.Join();
            Console.WriteLine("Independent task has completed, main thread ends.");
        }

        // The callback method must match the signature of the callback delegate.
        public static void ResultCallback(int lineCount) =>
            Console.WriteLine("Independent task printed {0} lines.", lineCount);
    }
    // The example displays the following output:
    //       Main thread does some work, then waits.
    //       This report displays the number 42.
    //       Independent task printed 1 lines.
    //       Independent task has completed; main thread ends.
}
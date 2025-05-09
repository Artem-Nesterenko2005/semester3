// <copyright file="MyThreadPool.cs" company="Artem-Nesterenko2005">
// Copyright (c) Artem-Nesterenko2005. All Rights Reserved.
// </copyright>
// <license>
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </license>

namespace ThreadPool;

using System.Collections.Concurrent;
using System.Threading;

/// <summary>
/// Class for threadpool performing tasks in parallel mode.
/// </summary>
public class MyThreadPool
{
    private Thread[] threadsArray;

    private ConcurrentQueue<Action> taskQueue;

    private CancellationTokenSource tokenSource;

    private CancellationToken token;

    private object lockObject;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyThreadPool"/> class.
    /// </summary>
    /// <param name="numberThreads">Number of threads in the threadpool.</param>
    public MyThreadPool(int numberThreads)
    {
        this.taskQueue = new ();
        this.lockObject = new ();
        this.tokenSource = new CancellationTokenSource();
        this.token = this.tokenSource.Token;

        this.threadsArray = new Thread[numberThreads];
        for (int i = 0; i < this.threadsArray.Length; ++i)
        {
            var localI = i;
            this.threadsArray[i] = new Thread(() =>
            {
                while (!this.token.IsCancellationRequested)
                {
                    lock (this.lockObject)
                    {
                        if (this.taskQueue.TryDequeue(out Action? task))
                        {
                            task.Invoke();
                        }
                        else
                        {
                            Monitor.Wait(this.lockObject);
                        }
                    }
                }

                while (this.taskQueue.TryDequeue(out Action? task))
                {
                    task.Invoke();
                }
            });
        }

        foreach (var thread in this.threadsArray)
        {
            thread.Start();
        }
    }

    /// <summary>
    /// Creates a new task by adding it to the threadpool queue.
    /// </summary>
    /// <typeparam name="TResult">Task data type.</typeparam>
    /// <param name="task">Task that are calculate.</param>
    /// <returns>Task that are executed threadpool.</returns>
    public IMyTask<TResult> Submit<TResult>(Func<TResult> task) => new MyTask<TResult>(task, this);

    /// <summary>
    /// Stops the threadpool work.
    /// </summary>
    public void Shutdown()
    {
        lock (this.lockObject)
        {
            this.tokenSource.Cancel();
            Monitor.PulseAll(this.lockObject);
        }

        foreach (var thread in this.threadsArray)
        {
            thread.Join();
        }
    }

    /// <summary>
    /// A class for tasks that are executed threadpool.
    /// </summary>
    /// <typeparam name="TResult">Task data type.</typeparam>
    private class MyTask<TResult> : IMyTask<TResult>
    {
        private MyThreadPool pool;

        private Func<TResult> task;

        private TResult? result;

        private ManualResetEvent resetEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyTask{TResult}"/> class.
        /// </summary>
        /// <param name="task">The function that needs to be performed.</param>
        /// <param name="pool">Threadpool performing the task.</param>
        public MyTask(Func<TResult> task, MyThreadPool pool)
        {
            this.task = task;
            this.IsCompleted = false;
            this.result = default;
            this.pool = pool;
            this.Exception = null;
            this.resetEvent = new (false);
            try
            {
                lock (pool.lockObject)
                {
                    if (pool.token.IsCancellationRequested)
                    {
                        throw new InvalidProgramException("Thread pool was interrupted");
                    }

                    pool.taskQueue.Enqueue(() => this.Calculate());
                    Monitor.PulseAll(pool.lockObject);
                }
            }
            catch (Exception ex)
            {
                this.Exception = ex;
            }
        }

        /// <summary>
        /// Gets a value indicating whether task completion status.
        /// </summary>
        public bool IsCompleted { get; private set; }

        public Exception? Exception { get; private set; }

        /// <summary>
        /// Gets the result of the task, if it was completed.
        /// </summary>
        public TResult Result => this.TaskResult() !;

        /// <summary>
        /// Calculates the value of the task.
        /// </summary>
        public void Calculate()
        {
            lock (this.pool.lockObject)
            {
                this.result = this.task.Invoke();
                this.IsCompleted = true;
                this.resetEvent.Set();
            }
        }

        /// <summary>
        /// Returns the next task to complete.
        /// </summary>
        /// <typeparam name="TNewResult">The data type of the new task.</typeparam>
        /// <param name="newTask">New task.</param>
        /// <returns>New task that are executed threadpool.</returns>
        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newTask)
        {
            if (!this.IsCompleted)
            {
                this.resetEvent.WaitOne();
            }

            return new MyTask<TNewResult>(() => newTask(this.Result), this.pool);
        }

        /// <summary>
        /// Returns the result of the completed task.
        /// </summary>
        /// <returns>The result of the task.</returns>
        /// <exception cref="AggregateException">h.</exception>
        private TResult? TaskResult()
        {
            this.resetEvent.WaitOne();
            lock (this.pool.lockObject)
            {
                if (this.Exception != null)
                {
                    throw new AggregateException(this.Exception);
                }

                return this.result;
            }
        }
    }
}

namespace ThreadPool
{
    // MIT License
    // Copyright (c) 2024 Artem-Nesterenko2005
    // All rights reserved

    using System.Collections.Concurrent;
    using System.ComponentModel;

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
                        if (this.taskQueue.TryDequeue(out Action? task))
                        {
                            task.Invoke();
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
        /// Add task to threadpool queue.
        /// </summary>
        /// <typeparam name="TResult">Task data type.</typeparam>
        /// <param name="myTask">Tasks that are executed threadpool.</param>
        /// <exception cref="InvalidProgramException">The exception thrown when threadpool was shutdown.</exception>
        public void AddTask<TResult>(MyTask<TResult> myTask)
        {
            lock (this.lockObject)
            {
                if (this.token.IsCancellationRequested)
                {
                    throw new InvalidProgramException("Thread pool was interrupted");
                }

                Action action = () =>
                {
                    myTask.Calculate();
                };

                this.taskQueue.Enqueue(action);
            }
        }

        /// <summary>
        /// creates a new task by adding it to the threadpool queue.
        /// </summary>
        /// <typeparam name="TResult">Task data type.</typeparam>
        /// <param name="task">Task that are calculate.</param>
        /// <returns>Task that are executed threadpool.</returns>
        public MyTask<TResult> Submit<TResult>(Func<TResult> task)
        => new MyTask<TResult>(task, this);

        /// <summary>
        /// Stops the threadpool work.
        /// </summary>
        public void Shutdown()
        {
            lock (this.lockObject)
            {
                this.tokenSource.Cancel();
            }

            foreach (var thread in this.threadsArray)
            {
                thread.Join();
            }
        }
    }
}

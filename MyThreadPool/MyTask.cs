namespace ThreadPool
{
    // MIT License
    // Copyright (c) 2024 Artem-Nesterenko2005
    // All rights reserved

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ThreadPool;

    /// <summary>
    /// A class for tasks that are executed threadpool.
    /// </summary>
    /// <typeparam name="TResult">Task data type.</typeparam>
    public class MyTask<TResult> : IMyTask<TResult>
    {
        private MyThreadPool pool;

        private Func<TResult> task;

        private TResult result;

        private object lockObject;

        private Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyTask{TResult}"/> class.
        /// </summary>
        /// <param name="task">The function that needs to be performed.</param>
        /// <param name="pool">Threadpool performing the task.</param>
        public MyTask(Func<TResult> task, MyThreadPool pool)
        {
            this.task = task;
            this.IsCompleted = false;
            this.result = default!;
            this.pool = pool;
            this.lockObject = new ();
            this.exception = null!;
            try
            {
                pool.AddTask(this);
            }
            catch (Exception ex)
            {
                this.exception = ex;
            }
        }

        /// <summary>
        /// Gets a value indicating whether task completion status.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Gets the result of the task, if it was completed.
        /// </summary>
        public TResult Result => this.TaskResult();

        /// <summary>
        /// Calculates the value of the task.
        /// </summary>
        public void Calculate()
        {
            lock (this.lockObject)
            {
                this.result = this.task.Invoke();
                this.IsCompleted = true;
                Monitor.PulseAll(this.lockObject);
            }
        }

        /// <summary>
        /// Returns the next task to complete.
        /// </summary>
        /// <typeparam name="TNewResult">The data type of the new task.</typeparam>
        /// <param name="newTask">New task.</param>
        /// <returns>New task that are executed threadpool.</returns>
        public MyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newTask)
        => new MyTask<TNewResult>(() => newTask(this.Result), this.pool);

        /// <summary>
        /// Returns the result of the completed task.
        /// </summary>
        /// <returns>The result of the task.</returns>
        /// <exception cref="AggregateException">h.</exception>
        private TResult TaskResult()
        {
            lock (this.lockObject)
            {
                while (!this.IsCompleted && this.exception == null!)
                {
                    Monitor.Wait(this.lockObject);
                }

                if (this.exception != null!)
                {
                    throw new AggregateException(this.exception);
                }

                return this.result;
            }
        }
    }
}

namespace ThreadPool
{
    // MIT License
    // Copyright (c) 2024 Artem-Nesterenko2005
    // All rights reserved

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for task executable threadpool.
    /// </summary>
    /// <typeparam name="TResult">Task data type.</typeparam>
    public interface IMyTask<TResult>
    {
        /// <summary>
        /// Gets a value indicating whether task completion status.
        /// </summary>
        public bool IsCompleted { get; }

        /// <summary>
        /// Gets the result of the task, if it was completed.
        /// </summary>
        public TResult Result { get; }

        /// <summary>
        /// Returns the next task to complete.
        /// </summary>
        /// <typeparam name="TNewResult">The data type of the new task.</typeparam>
        /// <param name="newTask">New task.</param>
        /// <returns>New task that are executed threadpool.</returns>
        public MyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newTask);
    }
}

// <copyright file="IMyTask.cs" company="Artem-Nesterenko2005">
// Copyright (c) Artem-Nesterenko2005. All Rights Reserved.
// </copyright>
// <license>
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </license>

namespace ThreadPool;

using System;

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
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newTask);
}

// <copyright file="SingleThreadLazy.cs" company="Artem-Nesterenko2005">
// Copyright (c) Artem-Nesterenko2005. All Rights Reserved.
// </copyright>
// <license>
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </license>

namespace Lazy;

/// <summary>
/// a class for lazy computation in single-threaded execution based on the interface ILazy.
/// </summary>
/// <typeparam name="T">the data type for lazy calculation.</typeparam>
public class SinglethreadLazy<T> : ILazy<T>
{
    /// <summary>
    /// a function that needs to be lazily calculated.
    /// </summary>
    private Func<T>? supplier;

    /// <summary>
    /// indicates whether the method has been called and whether the result has already been calculated.
    /// </summary>
    private bool calculated;

    /// <summary>
    /// the result of a lazy calculation.
    /// </summary>
    private T? result;

    /// <summary>
    /// Initializes a new instance of the <see cref="SinglethreadLazy{T}"/> class.
    /// </summary>
    /// <param name="function">function for lazy calculated.</param>
    public SinglethreadLazy(Func<T> function)
    {
        this.supplier = function;
        this.calculated = false;
        this.result = default;
    }

    /// <summary>
    /// checks whether the calculation of the function was performed.
    /// </summary>
    /// <returns>method has been called or not.</returns>
    public bool IsCalculated() => this.calculated;

    /// <summary>
    /// performs a function if it has not been performed before.
    /// </summary>
    /// <returns>the result of a lazy calculation.</returns>
    public T? Get()
    {
        if (!this.IsCalculated())
        {
            this.calculated = true;
            this.result = this.supplier();
            this.supplier = default;
        }

        return this.result;
    }
}

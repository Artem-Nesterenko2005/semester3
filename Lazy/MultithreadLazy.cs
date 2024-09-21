using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace Lazy
{
    /// <summary>
    /// a class for lazy computation in multithreaded execution based on the interface ILazy.
    /// </summary>
    /// <typeparam name="T">the data type for lazy calculation.</typeparam>
    public class MultithreadLazy<T> : ILazy<T>
    {
        /// <summary>
        /// a function that needs to be lazily calculated.
        /// </summary>
        private Func<T> supplier;

        /// <summary>
        /// indicates whether the method has been called and whether the result has already been calculated.
        /// </summary>
        private bool calculated;

        /// <summary>
        /// the result of a lazy calculation.
        /// </summary>
        private T result;

        /// <summary>
        /// object for locked thread.
        /// </summary>
        readonly private object lockObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultithreadLazy{T}"/> class.
        /// </summary>
        /// <param name="function">function for lazy calculated.</param>
        public MultithreadLazy(Func<T> function)
        {
            this.supplier = function;
            this.calculated = false;
            this.lockObject = new object();
            this.result = default!;
        }

        /// <summary>
        /// checks whether the calculation of the function was performed.
        /// </summary>
        /// <returns>method has been called or not.</returns>
        public bool IsCalculated() => this.calculated;

        /// <summary>
        /// performs a function if it has not been performed before with thread-safe execution.
        /// </summary>
        /// <returns>the result of a lazy calculation.</returns>
        public T Get()
        {
            lock (this.lockObject)
            {
                if (!this.IsCalculated())
                {
                    this.calculated = true;
                    this.result = this.supplier();
                    this.supplier = default!;
                }
            }

            return this.result;
        }
    }
}

// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace Lazy
{
    /// <summary>
    /// interface for lazy calculation.
    /// </summary>
    /// <typeparam name="T">the data type for lazy calculation.</typeparam>
    public interface ILazy<T>
    {
        /// <summary>
        /// performs a function if it has not been performed before.
        /// </summary>
        /// <returns>the result of a lazy calculation.</returns>
        public T Get();
    }
}

// <copyright file="ILazy.cs" company="Artem-Nesterenko2005">
// Copyright (c) Artem-Nesterenko2005. All Rights Reserved.
// </copyright>
// <license>
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </license>

namespace Lazy;

/// <summary>
/// interface for lazy calculation.
/// </summary>
/// <typeparam name="T">the data type for lazy calculation.</typeparam>
public interface ILazy<out T>
{
    /// <summary>
    /// performs a function if it has not been performed before.
    /// </summary>
    /// <returns>the result of a lazy calculation.</returns>
    public T? Get();
}

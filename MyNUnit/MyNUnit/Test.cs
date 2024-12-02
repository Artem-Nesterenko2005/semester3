// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Attributes;

namespace MyNUnit;

/// <summary>
/// Class for data about one specific test.
/// </summary>
public class Test
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Test"/> class.
    /// </summary>
    /// <param name="result">Result of the test.</param>
    /// <param name="time">Test execution time.</param>
    /// <param name="message">Message about ignoring or failing a test.</param>
    public Test(bool result, long time, string message)
    {
        this.Result = result;
        this.Time = time;
        this.Messages = message;
    }

    /// <summary>
    /// Gets a value indicating whether on result of the test.
    /// </summary>
    public bool Result { get; private set; }

    /// <summary>
    /// Gets a value indicating whether on test execution time.
    /// </summary>
    public long Time { get; private set; }

    /// <summary>
    /// Gets a value indicating whether on message about ignoring or failing a test.
    /// </summary>
    public string Messages { get; private set; }
}

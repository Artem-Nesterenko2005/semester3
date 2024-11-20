// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace Attributes;

/// <summary>
/// Attribute for marking tests.
/// </summary>
public class TestAttribute : Attribute
{
    /// <summary>
    /// Gets a exception expected to be received.
    /// </summary>
    public Type? Expected { get; private set; }

    /// <summary>
    /// Gets a reason for ignoring the test.
    /// </summary>
    public string? Ignore { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestsResult"/> class with the expected exception.
    /// </summary>
    /// <param name="expected">Exception expected to be received.</param>
    public TestAttribute(Type expected)
    {
        this.Expected = expected;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestsResult"/> class with the reason for ignoring.
    /// </summary>
    /// <param name="ignore">Reason for ignoring the test.</param>
    public TestAttribute(string ignore)
    {
        this.Ignore = ignore;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestsResult"/> class with the expected exception and the reason for ignoring.
    /// </summary>
    /// <param name="expected">Exception expected to be received.</param>
    /// <param name="ignore">Reason for ignoring the test.</param>
    public TestAttribute(Type expected, string ignore)
    {
        this.Expected = expected;
        this.Ignore = ignore;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestsResult"/> class without parameters.
    /// </summary>
    public TestAttribute()
    {
    }
}

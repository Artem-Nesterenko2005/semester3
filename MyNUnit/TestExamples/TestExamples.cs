// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

using System;
using Attributes;

namespace TestExamples;

public class Tests
{
    [Before]
    public void Before()
    {
    }

    [BeforeClass]
    public static void BeforeClass()
    {
    }

    [After]
    public void After()
    {
    }

    [AfterClass]
    public static void AfterClass()
    {
    }

    [Test]
    public void TestPass()
    {
        string passed = string.Empty;
    }

    [Test(typeof(IndexOutOfRangeException))]
    public void TestPassWithException()
    {
        throw new IndexOutOfRangeException();
    }

    [Test("ignore")]
    public void TestIgnored()
    {
        string ignore = string.Empty;
    }

    [Test]
    public void TestFail()
    {
        int[] array = new int[5] { 1, 2, 3, 4, 5 };
        var fail = array[7];
    }
}

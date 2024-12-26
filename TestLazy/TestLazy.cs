// <copyright file="TestLazy.cs" company="Artem-Nesterenko2005">
// Copyright (c) Artem-Nesterenko2005. All Rights Reserved.
// </copyright>
// <license>
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </license>

namespace TestLazy;
using Lazy;

public class Tests
{
    [Test]
    public void TestLazyCalculateFunction()
    {
        int Function() => 1 + 1;
        SinglethreadLazy<int> singleLazy = new (Function);
        MultithreadLazy<int> multiLazy = new (Function);
        Assert.That(singleLazy.Get(), Is.EqualTo(2));
        Assert.That(multiLazy.Get(), Is.EqualTo(2));
    }

    [Test]
    public void TestIsCalculated()
    {
        int Function() => 1 + 1;
        SinglethreadLazy<int> singleLazy = new (Function);
        MultithreadLazy<int> multiLazy = new (Function);
        singleLazy.Get();
        multiLazy.Get();
        Assert.IsTrue(singleLazy.IsCalculated());
        Assert.IsTrue(singleLazy.IsCalculated());
    }

    [Test]
    public void TestMultithread()
    {
        int Function() => 2 * 2;
        MultithreadLazy<int> multiLazy = new (Function);
        var threadsArray = new Thread[Environment.ProcessorCount];
        List<int> list = new ();
        for (int i = 0; i < threadsArray.Length; ++i)
        {
            int localI = i;
            threadsArray[localI] = new Thread(() =>
            {
                list.Add(multiLazy.Get());
            });
        }

        foreach (var thread in threadsArray)
        {
            thread.Start();
        }

        foreach (var thread in threadsArray)
        {
            thread.Join();
        }

        foreach (var symbol in list)
        {
            if (symbol != 4)
            {
                Assert.Fail();
            }
        }
    }
}

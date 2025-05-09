// <copyright file="TestMyThreadPool.cs" company="Artem-Nesterenko2005">
// Copyright (c) Artem-Nesterenko2005. All Rights Reserved.
// </copyright>
// <license>
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </license>

namespace TestMyThreadPool;

using System.Threading;
using ThreadPool;

public class Tests
{
    private MyThreadPool pool;

    [SetUp]
    public void Setup()
    {
        this.pool = new MyThreadPool(Environment.ProcessorCount);
    }

    [Test]
    public void TestCommonTask()
    {
        Assert.That(this.pool.Submit(() => 2 + 2).Result, Is.EqualTo(4));
    }

    [Test]
    public void TestContinueWithTask()
    {
        Assert.That(this.pool.Submit(() => 3 * 4).ContinueWith(x => x + 3).ContinueWith(x => x.ToString()).Result, Is.EqualTo("15"));
    }

    [Test]
    public void TestShutdownPool()
    {
        var result1 = this.pool.Submit(() => 2 + 2).Result;
        var result2 = this.pool.Submit(() => "axtr".Contains('x')).Result;
        var result3 = this.pool.Submit(() => 1.ToString()).Result;
        this.pool.Shutdown();
        var result4 = this.pool.Submit(() => 1 + 1);

        Assert.That(result1, Is.EqualTo(4));
        Assert.That(result2, Is.EqualTo(true));
        Assert.That(result3, Is.EqualTo("1"));
        Assert.That(result4.IsCompleted, Is.EqualTo(false));
    }

    [Test]
    public void TestNumberThreads()
    {
        var result = 0;
        for (int i = 0; i < 4; i++)
        {
            result = this.pool.Submit(() =>
            {
                Thread.Sleep(1000);
                return i;
            }).Result;
        }

        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void TestMultithreadPool()
    {
        var threads = new Thread[Environment.ProcessorCount];
        var result = new int[Environment.ProcessorCount];
        for (int i = 0; i < threads.Length; i++)
        {
            int localI = i;
            threads[i] = new Thread(() =>
            {
               result[localI] = this.pool.Submit(() => localI + localI).Result;
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        for (int i = 0; i < result.Length; ++i)
        {
            Assert.That(result[i], Is.EqualTo(i * 2));
        }
    }
}

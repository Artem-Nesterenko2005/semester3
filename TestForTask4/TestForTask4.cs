// <copyright file="TestForTask4.cs" company="Artem-Nesterenko2005">
// Copyright (c) Artem-Nesterenko2005. All Rights Reserved.
// </copyright>
// <license>
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </license>

namespace TestForTask4;
using TestTask4;

public class Tests
{
    private MultiThreadPriorityQueue<int> queue = new TestTask4.MultiThreadPriorityQueue<int>();

    [Test]
    public void TestPriorityEnqueue()
    {
        this.queue.Enqueue(1, 5);
        this.queue.Enqueue(2, 3);
        this.queue.Enqueue(3, 1);
        this.queue.Enqueue(4, 9);
        Assert.That(this.queue.Dequeue(), Is.EqualTo(4));
        Assert.That(this.queue.Dequeue(), Is.EqualTo(1));
        Assert.That(this.queue.Dequeue(), Is.EqualTo(2));
        Assert.That(this.queue.Dequeue(), Is.EqualTo(3));
    }

    [Test]
    public void TestEnqueueMultiThread()
    {
        Thread[] threads = new Thread[5];
        for (int i = 0; i < threads.Length; i++)
        {
            var localI = i;
            threads[i] = new Thread(() =>
            {
                this.queue.Enqueue(threads.Length - localI, localI);
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

        Assert.That(this.queue.Dequeue(), Is.EqualTo(1));
        Assert.That(this.queue.Dequeue(), Is.EqualTo(2));
        Assert.That(this.queue.Dequeue(), Is.EqualTo(3));
        Assert.That(this.queue.Dequeue(), Is.EqualTo(4));
        Assert.That(this.queue.Dequeue(), Is.EqualTo(5));
    }

    [Test]
    public void TestDequeueBeforeEnqueue()
    {
        Thread thread = new Thread(() =>
        {
            this.queue.Dequeue();
        });
        thread.Start();
        this.queue.Enqueue(1, 1);
        thread.Join();
        Assert.That(this.queue.Size, Is.EqualTo(0));
    }

    [Test]
    public void TestEqualPriorityEnqueue()
    {
        this.queue.Enqueue(5, 1);
        this.queue.Enqueue(6, 1);
        this.queue.Enqueue(7, 1);
        this.queue.Enqueue(8, 1);
        Assert.That(this.queue.Dequeue(), Is.EqualTo(5));
        Assert.That(this.queue.Dequeue(), Is.EqualTo(6));
        Assert.That(this.queue.Dequeue(), Is.EqualTo(7));
        Assert.That(this.queue.Dequeue(), Is.EqualTo(8));
    }
}

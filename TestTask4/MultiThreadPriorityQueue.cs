// <copyright file="MultiThreadPriorityQueue.cs" company="Artem-Nesterenko2005">
// Copyright (c) Artem-Nesterenko2005. All Rights Reserved.
// </copyright>
// <license>
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </license>

namespace TestTask4;

/// <summary>
/// Class for multithread priority queue with insert, delete and size check methods.
/// </summary>
/// <typeparam name="T">Data type for queue.</typeparam>
public class MultiThreadPriorityQueue<T>
{
    private QueueElement<T>? head = null;

    private object lockObject = new object();

    private int size;

    /// <summary>
    /// Gets a value indicating whether queue has not elements.
    /// </summary>
    public bool Empty => this.head == null;

    /// <summary>
    /// Gets number of queue elements.
    /// </summary>
    public int Size => this.size;

    /// <summary>
    /// Method for adding data to a queue.
    /// </summary>
    /// <param name="data">Data added to queue.</param>
    /// <param name="priority">Priority of added data.</param>
    public void Enqueue(T data, int priority)
    {
        lock (this.lockObject)
        {
            if (this.Empty)
            {
                this.head = new QueueElement<T>(data, priority, null);
                ++this.size;
                Monitor.PulseAll(this.lockObject);
                return;
            }

            if (this.head != null)
            {
                if (this.head.Priority < priority)
                {
                    QueueElement<T> node = new (data, priority, this.head);
                    this.head = node;
                    ++this.size;
                    Monitor.PulseAll(this.lockObject);
                    return;
                }

                var currentNode = this.head;
                while (currentNode.Next != null && currentNode.Next.Priority >= priority)
                {
                    currentNode = currentNode.Next;
                }

                QueueElement<T> newNode = new (data, priority, currentNode.Next);
                currentNode.Next = newNode;
                ++this.size;
                Monitor.PulseAll(this.lockObject);
            }
        }
    }

    /// <summary>
    /// Method that returns and removes the top value from the queue.
    /// </summary>
    /// <returns>Data from top of queue.</returns>
    /// <exception cref="EmptyPriorityQueueExceprion">Exception thrown when trying to remove an element from an empty queue.</exception>
    public T? Dequeue()
    {
        lock (this.lockObject)
        {
            while (this.size == 0)
            {
                Monitor.Wait(this.lockObject);
            }

            T? data = this.head.Data;
            this.head = this.head.Next;
            --this.size;
            return data;
        }
    }

    private class QueueElement<T>
    {
        /// <summary>
        /// Data from node.
        /// </summary>
        public T? Data;

        /// <summary>
        /// Priority from node.
        /// </summary>
        public int Priority;

        /// <summary>
        /// Next node of queue.
        /// </summary>
        public QueueElement<T>? Next;

        public QueueElement(T? data, int priority, QueueElement<T>? next)
        {
            this.Data = data;
            this.Priority = priority;
            this.Next = next;
        }
    }
}

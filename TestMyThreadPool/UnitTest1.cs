namespace TestMyThreadPool
{
    // MIT License
    // Copyright (c) 2024 Artem-Nesterenko2005
    // All rights reserved

    using System.Security.Cryptography.X509Certificates;
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

            MyTask<int> task = new MyTask<int>(() => 1 + 1, this.pool);
            Assert.Throws<InvalidProgramException>(() => this.pool.AddTask(task));
            Assert.IsTrue(result1 == 4);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3 == "1");
        }

        [Test]
        public void TestNumberThreads()
        {
            var result = 0;
            for (int i = 0; i < 4; i++)
            {
                result = this.pool.Submit(() => { Thread.Sleep(1000); return i; }).Result;
            }

            Assert.IsTrue(result == 3);
        }
    }
}

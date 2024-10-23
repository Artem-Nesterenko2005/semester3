using NuGet.Frameworks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using SimpleFTP;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;

// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace TestSimpleFTP
{
    [TestFixture]
    public class Tests
    {
        private Server server;

        private IPEndPoint endPoint;

        private Client client;

        private string filePath;

        private string directoryPath;

        private string rightResultDirectory;

        private string rightResultFile;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.endPoint = new (IPAddress.Loopback, 8888);
            this.client = new (this.endPoint);
            this.server = new (this.endPoint.Port);

            this.directoryPath = "1 testFiles\\";
            this.filePath = "2 testFiles\\file1.txt";
            this.server.Start();

            this.rightResultDirectory = "3 C:\\Users\\Huawei\\Desktop\\3sem\\SimpleFTP\\testFiles\\directory True " +
                "C:\\Users\\Huawei\\Desktop\\3sem\\SimpleFTP\\testFiles\\file1.txt False C:\\Users\\Huawei\\Desktop\\3sem\\SimpleFTP\\testFiles\\file2.txt False\n";

            this.rightResultFile = "3 123\n";
        }

        [Test]
        public void TestList()
        {
            Assert.That(this.client.Processing(this.directoryPath).Result, Is.EqualTo(this.rightResultDirectory));
        }

        [Test]
        public void TestGet()
        {
            Assert.That(this.client.Processing(this.filePath).Result, Is.EqualTo(this.rightResultFile));
        }

        [Test]
        public void TestNonExistentDirectory()
        {
            Assert.That(this.client.Processing("1 Non-existen").Result, Is.EqualTo("-1"));
        }

        [Test]
        public void TaskNonExistentFile()
        {
            Assert.That(this.client.Processing("2 Non_existen.txt").Result, Is.EqualTo("-1"));
        }

        [Test]
        public void TestLotTasks()
        {
            for (int i = 0; i < 100; ++i)
            {
                var result1 = this.client.Processing(this.filePath).Result;
                var result2 = this.client.Processing(this.directoryPath).Result;
                if (result1 != this.rightResultFile || result2 != this.rightResultDirectory)
                {
                    Assert.Fail();
                }
            }

            Assert.Pass();
        }

        [Test]
        public void TestUseServerAfterStop()
        {
            this.server.Stop();
            this.server.Start();
            Assert.That(this.client.Processing(this.directoryPath).Result, Is.EqualTo(this.rightResultDirectory));
            Assert.That(this.client.Processing(this.filePath).Result, Is.EqualTo(this.rightResultFile));
        }
    }
}

// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

using System.Reflection;
using System.Runtime.CompilerServices;
using MyNUnit;

namespace TestMyNUnit;

public class Tests
{
    private string path;

    [SetUp]
    public void Setup()
    {
        this.path = "../../../../TestExamples/bin/Debug/net8.0/";
    }

    [Test]
    public void TestInvalidPath()
    {
        Assert.Throws<InvalidDataException>(() => MyNUnit.MyNUnit.RunTests("../../../WrongPath/"));
    }

    [Test]
    public void TestMyNUnit()
    {
        Dictionary<string, bool> rightResults = new Dictionary<string, bool>
        {
            { "TestPass", true },
            { "TestPassWithException", true },
            { "TestIgnored", true },
            { "TestFail", false },
        };

        var result = MyNUnit.MyNUnit.RunTests(this.path).CreateReadOnlyTestsInfo();
        Dictionary<string, bool> receivedResults = new ();
        foreach (var testResult in result)
        {
            receivedResults.Add(testResult.Key.Name, testResult.Value.Result);
        }

        Assert.That(rightResults, Is.EqualTo(receivedResults));
    }
}

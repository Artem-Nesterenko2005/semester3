// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

using System.Collections;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using Attributes;

namespace MyNUnit;

/// <summary>
/// Class for storing and printing data about all tests.
/// </summary>
public class TestsData
{
    /// <summary>
    /// A read only dictionary containing pairs of methods labeled with attributes and instructions for adding these methods to a class.
    /// </summary>
    public IReadOnlyDictionary<Type, Action<MethodInfo, TestsData>> AttributeToAction = new Dictionary<Type, Action<MethodInfo, TestsData>>
    {
        { typeof(TestAttribute), (methodInfo, testData) => testData.tests.Add(methodInfo, null!) },
        { typeof(BeforeAttribute), (methodInfo, testsData) => testsData.BeforeTest = methodInfo },
        { typeof(AfterAttribute), (methodInfo, testsData) => testsData.AfterTest = methodInfo },
        { typeof(BeforeClassAttribute), (methodInfo, testsData) => testsData.BeforeClass = methodInfo },
        { typeof(AfterClassAttribute), (methodInfo, testsData) => testsData.AfterClass = methodInfo },
    };

    private Dictionary<MethodInfo, Test> tests;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestsData"/> class.
    /// </summary>
    public TestsData()
    {
        this.tests = new ();
    }

    /// <summary>
    /// Gets or sets method before running tests.
    /// </summary>
    public MethodInfo? BeforeTest { get; set; }

    /// <summary>
    /// Gets or sets method after running tests.
    /// </summary>
    public MethodInfo? AfterTest { get; set; }

    /// <summary>
    /// Gets or sets method before running tests in a class.
    /// </summary>
    public MethodInfo? BeforeClass { get; set; }

    /// <summary>
    /// Gets or sets method after running tests in a class.
    /// </summary>
    public MethodInfo? AfterClass { get; set; }

    /// <summary>
    /// Adds a test result by test name to the test dictionary.
    /// </summary>
    /// <param name="test">Test name.</param>
    /// <param name="result">Result of test.</param>
    public void AddResultTest(MethodInfo test, Test result) => this.tests[test] = result;

    /// <summary>
    /// Create read only dictionary based on dictionary with all tests.
    /// </summary>
    /// <returns>Read only dictionary based on dictionary tests.</returns>
    public IReadOnlyDictionary<MethodInfo, Test> CreateReadOnlyTestsInfo() => this.tests.ToDictionary(newKey => newKey.Key, newValue => newValue.Value).AsReadOnly();

    /// <summary>
    /// Adds tests to the dictionary with all tests.
    /// </summary>
    /// <param name="testData">Data about tests.</param>
    public void AddTests(TestsData testData)
    {
        foreach (var test in testData.tests)
        {
            this.tests.Add(test.Key, test.Value);
        }
    }

    /// <summary>
    /// Print results of all tests.
    /// </summary>
    public void PrintResult()
    {
        foreach (var test in this.tests)
        {
            Console.Write($"{test.Key.Name} | ");
            Console.Write($"Time: {test.Value!.Time} ms | ");
            if (test.Value.Result && test.Value.Messages != string.Empty)
            {
                Console.WriteLine($"Ignored - ({test.Value.Messages}) ");
            }
            else if (test.Value.Result && test.Value.Messages == string.Empty)
            {
                Console.WriteLine("Passed ");
            }
            else
            {
                Console.WriteLine($"Failed - {test.Value.Messages} ");
            }
        }
    }
}

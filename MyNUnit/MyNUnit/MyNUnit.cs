// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Globalization;
using Attributes;

namespace MyNUnit;

/// <summary>
/// Class for running unit tests.
/// </summary>
public static class MyNUnit
{
    /// <summary>
    /// Runs all tests that are located at the specified path.
    /// </summary>
    /// <param name="path">Path to file or directory with tests.</param>
    /// <returns>Results of all tests.</returns>
    /// <exception cref="InvalidDataException">Exception thrown when a directory or file on the path does not exist.</exception>
    public static TestsData RunTests(string path)
    {
        TestsData testsResult = new ();
        List<Task<TestsData>> tasks = new ();
        if (File.Exists(path))
        {
            tasks.Add(Task.Run(() => PerfomTest(path)));
        }
        else if (Directory.Exists(path))
        {
            foreach (var file in Directory.EnumerateFiles(path, "*.dll"))
            {
                tasks.Add(Task.Run(() => PerfomTest(file)));
            }
        }
        else
        {
            throw new InvalidDataException("Invalid path to directory");
        }

        foreach (var task in tasks)
        {
            testsResult.AddTests(task.Result);
        }

        return testsResult;
    }

    private static TestsData PerfomTest(string path)
    {
        TestsData testsResult = new ();
        var assembly = Assembly.LoadFrom(path);
        List<Task<TestsData>> tasks = new ();

        foreach (var type in assembly.GetExportedTypes())
        {
            tasks.Add(Task.Run(() =>
            {
                TestsData testsData = new ();
                foreach (var method in type.GetMethods())
                {
                    var attribute = Attribute.GetCustomAttributes(method);
                    if (attribute.Length != 0)
                    {
                        if (testsData.AttributeToAction.TryGetValue(attribute[0].GetType(), out var action))
                        {
                            action(method, testsData);
                        }
                    }
                }

                var readOnlyTestDictionary = testsData.CreateReadOnlyTestsInfo();
                var stopwatch = new Stopwatch();

                if (testsData.BeforeClass != null)
                {
                    testsData.BeforeClass.Invoke(type, null);
                }

                var instance = Activator.CreateInstance(type);
                foreach (var test in readOnlyTestDictionary)
                {
                    if (testsData.BeforeTest != null)
                    {
                        testsData.BeforeTest.Invoke(instance, null);
                    }

                    var attributeArguments = (TestAttribute)Attribute.GetCustomAttributes(test.Key)[0];
                    if (attributeArguments.Ignore != null)
                    {
                        testsData.AddResultTest(test.Key, new Test(true, 0, attributeArguments.Ignore));
                    }
                    else
                    {
                        try
                        {
                            stopwatch.Start();
                            test.Key.Invoke(instance, null);
                            stopwatch.Stop();
                            testsData.AddResultTest(test.Key, new Test(true, stopwatch.ElapsedMilliseconds, string.Empty));
                        }
                        catch (Exception exception)
                        {

                            stopwatch.Stop();
                            if (attributeArguments.Expected != default)
                            {
                                testsData.AddResultTest(test.Key, new Test(true, stopwatch.ElapsedMilliseconds, string.Empty));
                            }
                            else
                            {
                                testsData.AddResultTest(test.Key, new Test(false, stopwatch.ElapsedMilliseconds, exception.InnerException!.Message!));
                            }
                        }
                    }

                    if (testsData.AfterTest != null)
                    {
                        testsData.AfterTest.Invoke(instance, null);
                    }
                }

                if (testsData.AfterClass != null)
                {
                    testsData.AfterClass.Invoke(type, null);
                }
                return testsData;
            }));
        }

        foreach (var task in tasks)
        {
            testsResult.AddTests(task.Result);
        }

        return testsResult;
    }
}

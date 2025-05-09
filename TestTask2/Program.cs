// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace TestTask2;

/// <summary>
/// Main class.
/// </summary>
public class Program
{
    private static void Main()
    {
        Reflector.DiffClasses(typeof(ExampleClass), typeof(Reflector));
    }
}
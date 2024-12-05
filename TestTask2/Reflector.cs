// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace TestTask2;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// class for output of all methods, fields, interfaces, classes, comparison of classes by content.
/// </summary>
public class Reflector
{
    /// <summary>
    /// Analyze class and print in file all methods, fields, interfaces, classes.
    /// </summary>
    /// <param name="someClass">Class for analyze.</param>
    public static void PrintStructure(Type someClass)
    {
        string name = someClass.Name;
        string filePath = name + "File.cs";
        StreamWriter writer = new StreamWriter(filePath);

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assemblie in assemblies)
        {
            writer.WriteLine($"Assembly: {assemblie}");
            foreach (Type myType in assemblie.ExportedTypes)
            {
                var modifier = SearchModifier(myType);
                writer.WriteLine($"Modifier: {modifier}, Type: {myType}");
                foreach (MemberInfo info in myType.GetTypeInfo().DeclaredMembers)
                {
                    var typeName = info switch
                    {
                        FieldInfo _ => "FieldInfo",
                        MethodInfo _ => "MethodInfo",
                        ConstructorInfo _ => "ConstructorInfo",
                        _ => string.Empty,
                    };
                    writer.WriteLine($" {typeName}: {info}");
                }
            }
        }
    }

    /// <summary>
    /// Shows the difference in methods, fields.
    /// </summary>
    /// <param name="firstClass">First class for analyze.</param>
    /// <param name="secondClass">Second class for analyze.</param>
    public static void DiffClasses(Type firstClass, Type secondClass)
    {
        CompareMembers(firstClass.GetFields(), secondClass.GetFields());
        CompareMembers(firstClass.GetMethods(), secondClass.GetMethods());
    }

    private static string SearchModifier(Type someClass) => someClass.Attributes.ToString();

    private static void CompareMembers(MemberInfo[] first, MemberInfo[] second)
    {
        var firstElements = first.Select(info => info.Name + info.CustomAttributes);
        var secondElements = second.Select(info => info.Name + info.CustomAttributes);

        var onlyInA = firstElements.Except(secondElements);
        var onlyInB = secondElements.Except(firstElements);

        if (onlyInA.Any() || onlyInB.Any())
        {
            if (onlyInA.Any())
            {
                Console.WriteLine(first[0].DeclaringType?.Name + ":");
            }

            foreach (var member in onlyInA)
            {
                Console.WriteLine(member);
            }

            Console.WriteLine();

            if (onlyInB.Any())
            {
                Console.WriteLine(second[0].DeclaringType?.Name + ":");
            }

            foreach (var member in onlyInB)
            {
                Console.WriteLine(member);
            }
        }

        Console.WriteLine();
    }
}
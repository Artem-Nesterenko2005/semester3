using System.IO;
using System.Runtime.CompilerServices;

// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace MatrixMultiplication
{
    /// <summary>
    /// the class for running the program.
    /// </summary>
    public class Program
    {
        private static void Main()
        {
            Console.WriteLine("The path to the first matrix file");
            var firstPath = Console.ReadLine();
            if (!File.Exists(firstPath))
            {
                Console.WriteLine("No such file exists");
                return;
            }

            Console.WriteLine("The path to the second matrix file");
            var secondPath = Console.ReadLine();
            if (!File.Exists(secondPath))
            {
                Console.WriteLine("No such file exists");
                return;
            }

            Console.WriteLine("The path to the result matrix file");
            var resultPath = Console.ReadLine();

            Matrix firstMatrix = new(firstPath);
            Matrix secondMatrix = new(secondPath);
            var result = firstMatrix.MultiThreadMultiplicateMatrix(secondMatrix);
            result.WriteMatrixFile(resultPath);
        }
    }
}

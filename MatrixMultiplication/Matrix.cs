using System.Reflection.Metadata.Ecma335;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace MatrixMultiplication
{
    /// <summary>
    /// a class for a matrix in the form of a two-dimensional array containing methods for entering and outputting a matrix to a file,
    /// multiplying matrices in normal and parallel mode.
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// matrix in the form a two-dimensional array.
        /// </summary>
        readonly private int[,] matrix;

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class with reading matrix from file.
        /// </summary>
        /// <param name="filePath">the path to the matrix file.</param>
        public Matrix(string filePath)
        {
            this.matrix = this.ReadMatrixFromFile(filePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class with size of matrix.
        /// </summary>
        /// <param name="rows">the number of rows in the matrix.</param>
        /// <param name="columns">the number of column in the matrix.</param>
        public Matrix(int rows, int columns)
        {
            this.matrix = new int[rows, columns];
        }

        /// <summary>
        /// multiplicate two matrix.
        /// </summary>
        /// <param name="multiplier">the second matrix that the current one is multiplied by.</param>
        /// <returns>the matrix that is the result of multiplying the first by the second.</returns>
        /// <exception cref="MismatchRowsWithColumnsException">exclusion due to mismatch of first matrix rows and second matrix columns.</exception>
        public Matrix MultiplicateMatrix(Matrix multiplier)
        {
            var firstMatrix = this.matrix;
            var secondMatrixa = multiplier.matrix;
            if (firstMatrix.GetLength(1) != secondMatrixa.GetLength(0))
            {
                throw new MismatchRowsWithColumnsException("The number of columns of first matrix is not equal to the number of rows of second matrix");
            }

            Matrix result = new(firstMatrix.GetLength(0), secondMatrixa.GetLength(1));
            for (int i = 0; i < result.matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < secondMatrixa.GetLength(1); ++j)
                {
                    for (int t = 0; t < firstMatrix.GetLength(1); ++t)
                    {
                        result.matrix[i, j] += firstMatrix[i, t] * secondMatrixa[t, j];
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  multiplicate two matrix in parallel mode.
        /// </summary>
        /// <param name="multiplier">the second matrix that the current one is multiplied by.</param>
        /// <returns>the matrix that is the result of multiplying the first by the second.</returns>
        /// <exception cref="MismatchRowsWithColumnsException">exclusion due to mismatch of first matrix rows and second matrix columns.</exception>
        public Matrix MultiThreadMultiplicateMatrix(Matrix multiplier)
        {
            var firstMatrix = this.matrix;
            var secondMatrix = multiplier.matrix;
            if (firstMatrix.GetLength(1) != secondMatrix.GetLength(0))
            {
                throw new MismatchRowsWithColumnsException("The number of columns of first matrix is not equal to the number of rows of second matrix");
            }

            Matrix result = new(firstMatrix.GetLength(0), secondMatrix.GetLength(1));

            var threadsArray = new Thread[Environment.ProcessorCount];
            int pieceSize = 0;

            if (this.matrix.GetLength(0) > (threadsArray.Length + 1))
            {
                pieceSize = this.matrix.GetLength(0) / (threadsArray.Length + 1);
            }
            else
            {
                pieceSize = this.matrix.GetLength(0);
            }

            for (int k = 0; k < threadsArray.Length; ++k)
            {
                int localk = k;
                threadsArray[k] = new Thread(() =>
                {
                    for (int i = localk * pieceSize; i < (localk + 1) * pieceSize && i < this.matrix.GetLength(0); ++i)
                    {
                        for (int j = 0; j < secondMatrix.GetLength(1); ++j)
                        {
                            for (int t = 0; t < firstMatrix.GetLength(1); ++t)
                            {
                                result.matrix[i, j] += firstMatrix[i, t] * secondMatrix[t, j];
                            }
                        }
                    }
                });
            }

            foreach (var thread in threadsArray)
            {
                thread.Start();
            }

            foreach (var thread in threadsArray)
            {
                thread.Join();
            }

            return result;
        }

        /// <summary>
        /// randomize numbers for matrix.
        /// </summary>
        public void RandomizeMatrix()
        {
            Random random = new();
            for (var i = 0; i < this.matrix.Length; ++i)
            {
                this.matrix[i / this.matrix.GetLength(0), i % this.matrix.GetLength(0)] = random.Next() % 200;
            }
        }

        /// <summary>
        /// writes a matrix as a two-dimensional array into a file.
        /// </summary>
        /// <param name="filePath">the path to the recording file.</param>
        public void WriteMatrixFile(string filePath)
        {
            string data = null!;
            for (int i = 0; i < this.matrix.Length; ++i)
            {
                data += this.matrix[i / this.matrix.GetLength(1), i % this.matrix.GetLength(1)];
                if ((i + 1) % this.matrix.GetLength(1) == 0 && i != 0)
                {
                    data += "\r\n";
                    continue;
                }

                data += " ";
            }

            File.WriteAllText(filePath, data);
        }

        /// <summary>
        /// reads data from a file, creating a two-dimensional array matrix from it.
        /// </summary>
        /// <param name="filePath">the path to the matrix file.</param>
        /// <returns>matrix from file.</returns>
        /// <exception cref="InvalidSymbolException">exclude when an invalid character is detected.</exception>
        private int[,] ReadMatrixFromFile(string filePath)
        {
            string[] fileData = File.ReadAllText(filePath).Split(' ', '\n', '\r');
            int heightMatrix = 1;
            int lengthMatrix = 0;
            int number = 0;
            foreach (var symbol in fileData)
            {
                if (int.TryParse(symbol, out number))
                {
                    ++lengthMatrix;
                    continue;
                }

                if (symbol == string.Empty)
                {
                    ++heightMatrix;
                    continue;
                }

                throw new ArgumentException("Invalid symbol in file");
            }

            int[,] resultArray = new int[heightMatrix, lengthMatrix / heightMatrix];
            int indexMatrix = 0;
            for (int i = 0; i < fileData.Length; ++i)
            {
                if (int.TryParse(fileData[i], out number))
                {
                    resultArray[indexMatrix / resultArray.GetLength(1), indexMatrix++ % resultArray.GetLength(1)] = number;
                }
            }
            return resultArray;
        }
    }
}

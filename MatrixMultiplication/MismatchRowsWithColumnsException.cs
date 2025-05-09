// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace MatrixMultiplication
{
    /// <summary>
    /// class for exclusion due to mismatch of rows and columns.
    /// </summary>
    public class MismatchRowsWithColumnsException : SystemException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MismatchRowsWithColumnsException"/> class.
        /// </summary>
        /// <param name="text">text in exception.</param>
        public MismatchRowsWithColumnsException(string text)
            : base(text)
        {
        }
    }
}

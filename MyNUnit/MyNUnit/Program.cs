// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit;

class Program
{
    public static void Main()
    {
        var result = MyNUnit.RunTests("../../../../TestExamples/bin/Debug/net8.0/");
        result.PrintResult();
    }
}

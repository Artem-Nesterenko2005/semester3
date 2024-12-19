// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace TestTask3;

public class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length == 1)
        {
            await Server.ServerStart(int.Parse(args[0]));
        }
        else if (args.Length == 2)
        {
            await Client.ClientStart(int.Parse(args[0]), args[1]);
        }
    }
}
